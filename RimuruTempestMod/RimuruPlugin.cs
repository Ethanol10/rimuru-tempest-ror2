using BepInEx;
using RimuruMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Networking;
using EmotesAPI;
using BepInEx.Bootstrap;
using RimuruMod.SkillStates;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RimuruMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
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
        public const string MODVERSION = "0.1.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "POPCORN";

        public static RimuruPlugin instance;

        public RimuruController Rimurucon;
        public RimuruMasterController Rimurumastercon;

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
            new RimuruHuman().Initialize(false);
            new RimuruSlime().Initialize(true);

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.CharacterModel.Start += CharacterModel_Start;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

            if (Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }



        //swordPosition
        private void CharacterModel_Start(On.RoR2.CharacterModel.orig_Start orig, CharacterModel self)
        {
            orig(self);
            //glitching out sword position, maybe one day
            //if (self.gameObject.name.Contains("RimuruHumanDisplay"))
            //{
            //    RimuruSwordDisplayController displayHandler = self.gameObject.GetComponent<RimuruSwordDisplayController>();
            //    if (!displayHandler)
            //    {
            //        ChildLocator childLoc = self.gameObject.GetComponent<ChildLocator>();

            //        if (childLoc)
            //        {
            //            Transform swordMesh = childLoc.FindChild("SwordMeshPosition");
            //            Transform swordsheatheTrans = childLoc.FindChild("SwordPosition");

            //            displayHandler = self.gameObject.AddComponent<RimuruSwordDisplayController>();
            //            displayHandler.swordTargetTransform = swordsheatheTrans;
            //            displayHandler.swordTransform = swordMesh;
            //        }
            //    }
            //}
        }

        //EMOTES
        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            foreach (var item in SurvivorCatalog.allSurvivorDefs)
            {
                if (item.bodyPrefab.name == "RimuruHumanBody")
                {
                    CustomEmotesAPI.ImportArmature(item.bodyPrefab, Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("EmoteRimuru"));
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

                if (self.body.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUHUMAN_BODY_NAME" || 
                    self.body.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
                {
                    bool flag = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
                    if (!flag && damageInfo.damage > 0f)
                    {
                        //resistance buff
                        if (self.body.HasBuff(Modules.Buffs.ResistanceBuff.buffIndex))
                        {
                            if (self.combinedHealthFraction < 0.5f && (damageInfo.damageType & DamageType.DoT) != DamageType.DoT)
                            {
                                damageInfo.force = Vector3.zero;
                                damageInfo.damage -= self.body.armor;
                                if (damageInfo.damage < 0f)
                                {
                                    self.Heal(Mathf.Abs(damageInfo.damage), default(RoR2.ProcChainMask), true);
                                    damageInfo.damage = 0f;

                                }

                            }
                            else
                            {
                                damageInfo.force = Vector3.zero;
                                damageInfo.damage = Mathf.Max(1f, damageInfo.damage - self.body.armor);
                            }
                        }
                        //regen buff
                        if (self.body.HasBuff(Modules.Buffs.RegenerationBuff.buffIndex))
                        {
                            if ((damageInfo.damageType & DamageType.DoT) != DamageType.DoT)
                            {
                                int missinghealth = Mathf.RoundToInt((damageInfo.damage * 5)/ self.combinedHealth); 
                                if (missinghealth > 0)
                                {
                                    self.body.ApplyBuff(Modules.Buffs.RegenstackBuff.buffIndex, missinghealth, -1);
                                }

                            }
                            else
                            {
                                damageInfo.force = Vector3.zero;
                                damageInfo.damage = Mathf.Max(1f, damageInfo.damage - self.body.armor);
                            }
                        }
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
                    //shock on wet interaction
                    if (victimBody.HasBuff(Modules.Buffs.WetDebuff) && !victimBody.HasBuff(Modules.Buffs.WetLightningDebuff))
                    {
                        if (damageInfo.damage > 0 && damageInfo.damageType == DamageType.Shock5s)
                        {
                            victimBody.ApplyBuff(Modules.Buffs.WetLightningDebuff.buffIndex, 1, 1);

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
                    //fire on wet interaction
                    if (victimBody.HasBuff(Modules.Buffs.WetDebuff))
                    {
                        if (damageInfo.damage > 0 && damageInfo.damageType == DamageType.IgniteOnHit)
                        {
                            victimBody.ApplyBuff(Modules.Buffs.WetDebuff.buffIndex, 0, 0);

                            EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
                            {
                                origin = victimBody.transform.position,
                                scale = Modules.StaticValues.lemurianfireRadius * body.attackSpeed / 2
                            }, true);

                            new BlastAttack
                            {
                                attacker = damageInfo.attacker.gameObject,
                                teamIndex = TeamComponent.GetObjectTeam(damageInfo.attacker.gameObject),
                                falloffModel = BlastAttack.FalloffModel.None,
                                baseDamage = body.damage * Modules.StaticValues.lemurianfireDamageCoefficient,
                                damageType = DamageType.IgniteOnHit,
                                damageColorIndex = DamageColorIndex.Fragile,
                                baseForce = 0,
                                position = victimBody.transform.position,
                                radius = Modules.StaticValues.lemurianfireRadius * body.attackSpeed / 2,
                                procCoefficient = 1f,
                                attackerFiltering = AttackerFiltering.NeverHitSelf,
                            }.Fire();


                        }
                    }
                    //lightning on fire interaction
                    if (victimBody.HasBuff(RoR2Content.Buffs.OnFire))
                    {
                        if (damageInfo.damage > 0 && damageInfo.damageType == DamageType.Shock5s)
                        {
                            int buffcount = victimBody.GetBuffCount(RoR2Content.Buffs.OnFire);

                            EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
                            {
                                origin = victimBody.transform.position,
                                scale = Modules.StaticValues.lemurianfireRadius/15 * buffcount
                            }, true);

                            new BlastAttack
                            {
                                attacker = damageInfo.attacker.gameObject,
                                teamIndex = TeamComponent.GetObjectTeam(damageInfo.attacker.gameObject),
                                falloffModel = BlastAttack.FalloffModel.None,
                                baseDamage = body.damage * buffcount,
                                damageType = DamageType.Stun1s,
                                damageColorIndex = DamageColorIndex.WeakPoint,
                                baseForce = 0,
                                position = victimBody.transform.position,
                                radius = Modules.StaticValues.lemurianfireRadius/15 * buffcount,
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

            if (self.HasBuff(Modules.Buffs.ResistanceBuff))
            {
                self.armor += 10f;
            }
            if (self.HasBuff(Modules.Buffs.SpatialMovementBuff))
            {
                self.armor += 300f;
            }
            if (self.HasBuff(Modules.Buffs.StrengthBuff))
            {
                self.damage *= 1.5f;
            }
            if (self.HasBuff(Modules.Buffs.CritDebuff))
            {
                AnalyzeEffectController analyzecontroller = self.gameObject.GetComponent<AnalyzeEffectController>();
                if (!analyzecontroller)
                {
                    analyzecontroller = self.gameObject.AddComponent<AnalyzeEffectController>();
                    analyzecontroller.charbody = self;
                }
            }
            if (self.HasBuff(Modules.Buffs.WetDebuff))
            {
                WetEffectController wetcontroller = self.gameObject.GetComponent<WetEffectController>();
                if (!wetcontroller)
                {
                    wetcontroller = self.gameObject.AddComponent<WetEffectController>();
                    wetcontroller.charbody = self;
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