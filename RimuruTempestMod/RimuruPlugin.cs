using BepInEx;
using RimuruMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RimuruMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class RimuruPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.PopcornFactory.RimuruTempestMod";
        public const string MODNAME = "RimuruTempestMod";
        public const string MODVERSION = "0.0.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "POPCORN";

        public static RimuruPlugin instance;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new RimuruHuman().Initialize();
            new RimuruSlime().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage; ;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig.Invoke(self, damageReport);
            //devour check
            if(damageReport.attackerBody.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
            {
                if(damageReport.damageInfo.damageType == DamageType.BonusToLowHealth)
                {
                    var name = BodyCatalog.GetBodyName(damageReport.victimBody.healthComponent.body.bodyIndex);
                    GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);
                    if (newbodyPrefab.name == "BeetleBody")
                    {
                        Chat.AddMessage("<style=cIsUtility>Beetle Skill</style> Get!");
                    }
                }

            }

        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>())
            {
                //crit buff
                if (self.GetComponent<CharacterBody>().HasBuff(Modules.Buffs.CritDebuff))
                {
                    if ((damageInfo.damageType & DamageType.DoT) != DamageType.DoT)
                    {
                        damageInfo.crit = true;

                    }
                }
            }

            orig.Invoke(self, damageInfo);
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            var attacker = damageInfo.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();
                if (body && victimBody)
                {
                    //wet and shock interaction
                    if (victimBody.HasBuff(Modules.Buffs.WetLightningDebuff) && !victimBody.HasBuff(Modules.Buffs.WetLightningDebuff))
                    {
                        if (damageInfo.damage > 0 && damageInfo.damageType == DamageType.Shock5s)
                        {
                            victimBody.AddTimedBuffAuthority(Modules.Buffs.WetLightningDebuff.buffIndex, 1);

                            EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova"), new EffectData
                            {
                                origin = victimBody.transform.position,
                                scale = Modules.StaticValues.blacklightningRadius * body.attackSpeed/2
                            }, true);
                                
                            new BlastAttack
                            {
                                attacker = damageInfo.attacker.gameObject,
                                teamIndex = TeamComponent.GetObjectTeam(damageInfo.attacker.gameObject),
                                falloffModel = BlastAttack.FalloffModel.None,
                                baseDamage = body.damage * Modules.StaticValues.blacklightningDamageCoefficient,
                                damageType = DamageType.Shock5s,
                                damageColorIndex = DamageColorIndex.WeakPoint,
                                baseForce = 0,
                                position = victimBody.transform.position,
                                radius = Modules.StaticValues.blacklightningRadius * body.attackSpeed/2,
                                procCoefficient = 1f,
                                attackerFiltering = AttackerFiltering.NeverHitSelf,
                            }.Fire();
                            

                        }
                    }
                }
            }

            orig.Invoke(self, damageInfo, victim);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.SpatialMovementBuff))
                {
                    self.armor += 300f;
                }
            }
        }
        private void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);

            if (self)
            {
                if (self.body)
                {
                    this.OverlayFunction(Modules.Assets.SpatialMovementBuffMaterial, self.body.HasBuff(Modules.Buffs.SpatialMovementBuff), self);
                }
            }
        }

        private void OverlayFunction(Material overlayMaterial, bool condition, CharacterModel model)
        {
            if (model.activeOverlayCount >= CharacterModel.maxOverlays)
            {
                return;
            }
            if (condition)
            {
                Material[] array = model.currentOverlays;
                int num = model.activeOverlayCount;
                model.activeOverlayCount = num + 1;
                array[num] = overlayMaterial;
            }
        }
    }
}