﻿using BepInEx;
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
using RimuruMod.Modules;
using RimuruMod.Modules.Networking;
using RimuruMod.Modules.Networking;
using RimuruMod.Content.BuffControllers;
using R2API;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RimuruMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.prefab", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.language", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.sound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.networking", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    public class RimuruPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.PopcornFactory.RimuruTempestMod";
        public const string MODNAME = "RimuruTempestMod";
        public const string MODVERSION = "1.1.2";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "POPCORN";

        public static RimuruPlugin instance;

        public RimuruController Rimurucon;
        public RimuruMasterController Rimurumastercon;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.AssetsRimuru.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            if (Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions")) //risk of options support
            {
                Modules.Config.SetupRiskOfOptions();
            }
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            Modules.Damage.SetupModdedDamage();

            // survivor initialization
            new RimuruHuman().Initialize(false);
            new RimuruSlime().Initialize(true);

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Modules.StaticValues.LoadDictionary();

            //networking
            NetworkingAPI.RegisterMessageType<ItemDropNetworked>();
            NetworkingAPI.RegisterMessageType<HealNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PeformDirectionalForceNetworkRequest>();
            NetworkingAPI.RegisterMessageType<SetDashStateMachine>();
            NetworkingAPI.RegisterMessageType<GravityPulsePullRequest>();
            NetworkingAPI.RegisterMessageType<OrbDamageRequest>();
            NetworkingAPI.RegisterMessageType<UpdateControllers>();


            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            //On.RoR2.CharacterModel.Start += CharacterModel_Start;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;

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
                    CustomEmotesAPI.ImportArmature(item.bodyPrefab, Modules.AssetsRimuru.mainAssetBundle.LoadAsset<GameObject>("EmoteRimuru"));
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self)
            {
                if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>())
                {
                    //crit buff
                    if (self.GetComponent<CharacterBody>().HasBuff(Modules.Buffs.CritDebuff) && self.body.teamComponent.teamIndex != TeamIndex.Player)
                    {
                        if ((damageInfo.damageType.damageType & DamageType.DoT) != DamageType.DoT)
                        {

                            if (Modules.Config.doubleInsteadOfCrit.Value)
                            {
                                damageInfo.damage *= 2.0f;
                            }
                            else
                            {
                                damageInfo.crit = true;
                            }
                        }
                    }

                    if (damageInfo.attacker.GetComponent<CharacterBody>().teamComponent.teamIndex == TeamIndex.Player) 
                    {
                        if (damageInfo.attacker.GetComponent<CharacterBody>().HasBuff(Modules.Buffs.CritDebuff)) 
                        {
                            if ((damageInfo.damageType.damageType & DamageType.DoT) != DamageType.DoT)
                            {

                                if (Modules.Config.doubleInsteadOfCrit.Value)
                                {
                                    damageInfo.damage *= 2.0f;
                                }
                                else
                                {
                                    damageInfo.crit = true;
                                }
                            }
                        }
                    }


                    //modded damage type for devour
                    if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.rimuruDevour))
                    {
                        self.body.ApplyBuff(Buffs.rimuruDevourDebuff.buffIndex, 1, -1);
                    }

                }

                if (damageInfo != null) 
                {
                    if (self.body.HasBuff(Modules.Buffs.immuneToFallDamage)) 
                    {
                        DamageType tempDamageType = DamageType.FallDamage | DamageType.NonLethal;
                        DamageType frailtyDamageType = DamageType.FallDamage | DamageType.BypassOneShotProtection;

                        if (damageInfo.damageType.damageType == tempDamageType || damageInfo.damageType.damageType == frailtyDamageType)
                        {
                            damageInfo.damage = 0f;
                            damageInfo.rejected = true;
                        }
                    }
                }

            }

            orig(self, damageInfo);
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig.Invoke(self, damageInfo, victim);

            if (damageInfo.attacker)
            {
                var body = damageInfo.attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();
                if (body && victimBody)
                {
                    //shock on wet interaction
                    if (victimBody.HasBuff(Modules.Buffs.WetDebuff) && !victimBody.HasBuff(Modules.Buffs.WetLightningDebuff))
                    {
                        if (damageInfo.damage > 0 && damageInfo.damageType.damageType == DamageType.Shock5s)
                        {
                            victimBody.ApplyBuff(Modules.Buffs.WetLightningDebuff.buffIndex, 1, 1);

                            EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova"), new EffectData
                            {
                                origin = victimBody.transform.position,
                                scale = Modules.Config.blackLightningRadius.Value * body.attackSpeed/2
                            }, true);
                                
                            new BlastAttack
                            {
                                attacker = damageInfo.attacker.gameObject,
                                teamIndex = TeamComponent.GetObjectTeam(damageInfo.attacker.gameObject),
                                falloffModel = BlastAttack.FalloffModel.None,
                                baseDamage = body.damage * Modules.Config.blackLightningDamageCoefficient.Value,
                                damageType = new RoR2.DamageTypeCombo(DamageType.Shock5s, DamageTypeExtended.Generic, DamageSource.NoneSpecified),
                                damageColorIndex = DamageColorIndex.WeakPoint,
                                baseForce = 0,
                                position = victimBody.transform.position,
                                radius = Modules.Config.blackLightningRadius.Value * body.attackSpeed/2,
                                procCoefficient = 1f,
                                attackerFiltering = AttackerFiltering.NeverHitSelf,
                            }.Fire();
                            

                        }
                    }
                    //fire on wet interaction
                    if (victimBody.HasBuff(Modules.Buffs.WetDebuff))
                    {
                        if (damageInfo.damage > 0 && damageInfo.damageType.damageType == DamageType.IgniteOnHit)
                        {
                            victimBody.ApplyBuff(Modules.Buffs.WetDebuff.buffIndex, 0, 0);

                            EffectManager.SpawnEffect(Modules.AssetsRimuru.elderlemurianexplosionEffect, new EffectData
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
                                damageType = new RoR2.DamageTypeCombo(DamageType.IgniteOnHit, DamageTypeExtended.Generic, DamageSource.NoneSpecified),
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
                        if (damageInfo.damage > 0 && damageInfo.damageType.damageType == DamageType.Shock5s)
                        {
                            int buffcount = victimBody.GetBuffCount(RoR2Content.Buffs.OnFire);

                            EffectManager.SpawnEffect(Modules.AssetsRimuru.elderlemurianexplosionEffect, new EffectData
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
                                damageType = new RoR2.DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.NoneSpecified),
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

        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            if (self?.healthComponent) 
            {
                orig(self);
                if (self)
                {
                    //if (self.HasBuff(Modules.Buffs.resistanceBuff))
                    //{
                    //    self.armor += StaticValues.resBuffArmor;
                    //}
                    if (self.HasBuff(Modules.Buffs.tarManipDebuff))
                    {
                        self.attackSpeed *= StaticValues.tarManipCoefficient;
                    }
                    if (self.HasBuff(Modules.Buffs.SpatialMovementBuff))
                    {
                        self.armor += StaticValues.spatialmovementbuffArmor;
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
            }
        }

        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME" || self.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUHUMAN_BODY_NAME")
            {
                AkSoundEngine.PostEvent("RimuruDeath", self.gameObject);
            }
        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("RimuruHumanDisplay"))
            {
                AkSoundEngine.PostEvent("RimuruEntrance", self.gameObject);
            }
        }

        private void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);

            if (self)
            {
                if (self.body)
                {
                    this.OverlayFunction(Modules.AssetsRimuru.SpatialMovementBuffMaterial, self.body.HasBuff(Modules.Buffs.SpatialMovementBuff), self);
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