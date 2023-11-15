using RimuruMod.Modules.Survivors;
using RimuruTempestMod.Content.BuffControllers;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimuruMod.Modules
{
    internal static class StaticValues
    {
        //passives

        internal const float refreshTimerDuration = 60f;

        //strength
        internal const float strengthBuffCoefficient = 1.5f;

        //speed
        internal const float speedBuffCoefficient = 1.2f;

        //attack speed
        internal const float attackSpeedBuffCoefficient = 1.2f;

        //lifesteal
        internal const float lifestealBuffCoefficient = 0.1f;

        //ultraspeed
        internal const float ultraspeedRegenCoefficient = 0.5f;
        internal const float ultraspeedHealthThreshold = 0.1f;
        internal const int ultraspeedBuffStacks = 5;

        //lightning
        internal const float lightningPulseTimer = 2f;

        //body armor
        internal const float bodyArmorCoefficient = 0.1f;

        //melee attacks
        internal const float swordDamageCoefficient = 2f;
        internal const float devourDamageCoefficient = 1f;

        //blacklightning
        internal const float blacklightningtotalDuration = 4f;
        internal const float blacklightningDamageCoefficient = 1f;
        internal const float blacklightningProcCoefficient = 0.5f;
        internal const float blacklightningRadius = 10f;

        //spatial movement
        internal const int spatialmovementbuffDuration = 3;
        internal const float spatialmovementbuffArmor = 50f;

        //analyze
        internal const int analyzedebuffDuration = 6;

        //Waterblade parameters
        internal const float waterbladeDamageCoefficient = 2.0f;
        internal const float waterbladeForce = 100f;
        internal const float waterbladeProjectileLifetime = 2.0f;
        internal const float waterbladeProjectileSpeed = 20f;

        //Wet debuff Params
        internal const float wetDebuffLen = 6.0f;

        //lemurian fire buff
        internal const float lemurianfireDamageCoefficient = 4f;
        internal const float lemurianfireProcCoefficient = 1f;
        internal const float lemurianfireRadius = 15f;
    
        //Force
        internal const float forcepushDamageCoefficient = 3.5f;
        internal const float forcepullDamageCoefficient = 4f;
        internal static float forceMaxRange = 100f;
        internal static float forceMaxTrackingAngle = 30f;
        //nullifier big brain
        internal const int nullifierBigBrainThreshold= 4;

        //scavenger creation
        internal const int tier1Amount = 4;
        internal const int tier2Amount = 2;
        internal const int tier3Amount = 1;

        //lunar exploder luck manipulation
        internal const int luckAmount = 1;

        //hermit crab mortar
        internal const float hermitMortarRadius = 40f;
        internal const float mortarbaseDuration = 1f;
        internal const float mortarDamageCoefficient = 1.5f;

        //aoe buffer 
        internal const float aoeBufferRadius = 40f;

        //gravity manipulation
        internal const float gravManipulationRadius = 40f;
        internal const float gravManipulationDamageCoefficient = 1f;
        internal const float gravManipulationForce = 10f;
        internal const float gravManipulationThreshold = 1f;

        //flight
        internal const float flightBuffThreshold = 3.5f;

        //flame body
        internal const float flameBodyRadius = 40f;
        internal const float flameBodyDuration = 4f;
        internal const float flameBodyDamageCoefficient = 2f;
        internal const float flameBodyThreshold = 2f;

        //tar manip
        internal const float tarManipRadius = 30f;
        internal const float tarManipCoefficient = 0.7f;

        //reparation
        internal const float reparationCoefficient = 0.5f;
        internal const float reparationTimer = 5f;

        //dash
        internal const float dashDuration = 0.3f;
        internal const float dashSpeedCoefficient = 5f;

        //tarring
        internal const float tarringRadius = 30f;
        internal const float tarringDamageCoefficient = 1.5f;

        //hyper regen
        internal const float hyperRegenCoefficient = 0.01f;

        //gravity pulse
        internal const float gravityPulseRange = 20f;

        //spiked body
        internal const float spikedBodyRange = 10f;

        //cleanser
        internal const int cleanserInterval = 10;

        //refraction
        internal const int refractionBounces = 4;
        internal const float refractionRange = 20f;

        //singular barrier
        internal const int singularBarrierInterval = 10;

        //reverse grav manip
        internal const int reverseGravManipInterval = 10;
        internal const float reverseGravManipRange = 50f;
        internal const float reverseGravManipDamageCoefficient = 2f;

        //Dictionary containing all created skills for rimuru.
        public static Dictionary<string, Func<CharacterMaster, RimuruBaseBuffController>> rimDic;

        //Wrapper class that allows templating 
        class BuffWrapperClass<T> where T: RimuruBaseBuffController
        {
            public static T AddBuffComponentToMaster(CharacterMaster master) 
            {
                Debug.Log(master.ToString());   
                T returnObj = master.gameObject.GetComponent<T>();
                returnObj = returnObj ? returnObj : master.gameObject.AddComponent<T>();
                returnObj.RefreshTimers();
                return returnObj;
            }
        }

        public static void LoadDictionary()
        {
            // CharacterMaster is input, RimuruBaseBuffController is output. Instantiating dictionary at beginning.
            rimDic = new Dictionary<string, Func<CharacterMaster, RimuruBaseBuffController>>
            {
                { "MinorConstructBody", (CharacterMaster master) => BuffWrapperClass<AlphaConstructBuffController>.AddBuffComponentToMaster(master) },
                { "MinorConstructOnKillBody", (CharacterMaster master) => BuffWrapperClass<AlphaConstructBuffController>.AddBuffComponentToMaster(master) },
                { "BeetleBody", (CharacterMaster master) => BuffWrapperClass<BeetleBuffController>.AddBuffComponentToMaster(master) },
                { "FlyingVerminBody", (CharacterMaster master) => BuffWrapperClass<FlyingVerminBuffController>.AddBuffComponentToMaster(master) },
                { "VerminBody", (CharacterMaster master) => BuffWrapperClass<BlindVerminBuffController>.AddBuffComponentToMaster(master) },
                { "GupBody", (CharacterMaster master) => BuffWrapperClass<GupBuffController>.AddBuffComponentToMaster(master) },
                { "GipBody", (CharacterMaster master) => BuffWrapperClass<GupBuffController>.AddBuffComponentToMaster(master) },
                { "GeepBody", (CharacterMaster master) => BuffWrapperClass<GupBuffController>.AddBuffComponentToMaster(master) },
                { "HermitCrabBody", (CharacterMaster master) => BuffWrapperClass<HermitCrabBuffController>.AddBuffComponentToMaster(master) },
                { "AcidLarvaBody", (CharacterMaster master) => BuffWrapperClass<AcidLarvaBuffController>.AddBuffComponentToMaster(master) },
                { "wispBody", (CharacterMaster master) => BuffWrapperClass<WispBuffController>.AddBuffComponentToMaster(master) },
                { "LunarExploderBody", (CharacterMaster master) => BuffWrapperClass<LunarExploderBuffController>.AddBuffComponentToMaster(master) },
                { "MiniMushroomBody", (CharacterMaster master) => BuffWrapperClass<MushrumBuffController>.AddBuffComponentToMaster(master) },
                { "RoboBallMiniBody", (CharacterMaster master) => BuffWrapperClass<SolusProbeBuffController>.AddBuffComponentToMaster(master) },
                { "RoboBallGreenBuddyBody", (CharacterMaster master) => BuffWrapperClass<SolusProbeBuffController>.AddBuffComponentToMaster(master) },
                { "RoboBallRedBuddyBody", (CharacterMaster master) => BuffWrapperClass<SolusProbeBuffController>.AddBuffComponentToMaster(master) },
                { "VoidBarnacleBody", (CharacterMaster master) => BuffWrapperClass<VoidBarnacleBuffController>.AddBuffComponentToMaster(master) },
                { "VoidJailerBody", (CharacterMaster master) => BuffWrapperClass<VoidJailerBuffController>.AddBuffComponentToMaster(master) },
                { "ImpBossBody", (CharacterMaster master) => BuffWrapperClass<ImpBossBuffController>.AddBuffComponentToMaster(master) },
                { "TitanBody", (CharacterMaster master) => BuffWrapperClass<TitanBuffController>.AddBuffComponentToMaster(master) },
                { "TitanGoldBody", (CharacterMaster master) => BuffWrapperClass<TitanBuffController>.AddBuffComponentToMaster(master) },
                { "VagrantBody", (CharacterMaster master) => BuffWrapperClass<WanderingVagrantBuffController>.AddBuffComponentToMaster(master) },
                { "MagmaWormBody", (CharacterMaster master) => BuffWrapperClass<MagmaWormBuffController>.AddBuffComponentToMaster(master) },
                { "ElectricWormBody", (CharacterMaster master) => BuffWrapperClass<OverloadingWormBuffController>.AddBuffComponentToMaster(master) },
                { "VultureBody", (CharacterMaster master) => BuffWrapperClass<VultureBuffController>.AddBuffComponentToMaster(master) },
                { "BeetleGuardBody", (CharacterMaster master) => BuffWrapperClass<BeetleGuardBuffController>.AddBuffComponentToMaster(master) },
                { "BisonBody", (CharacterMaster master) => BuffWrapperClass<BisonBuffController>.AddBuffComponentToMaster(master) },
                { "ClayGrenadierBody", (CharacterMaster master) => BuffWrapperClass<ClayApothecaryBuffController>.AddBuffComponentToMaster(master) },
                { "BellBody", (CharacterMaster master) => BuffWrapperClass<BronzongBuffController>.AddBuffComponentToMaster(master) },
                { "ClayBruiserBody", (CharacterMaster master) => BuffWrapperClass<ClayTemplarBuffController>.AddBuffComponentToMaster(master) },
                { "LemurianBruiserBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master) },
                { "GreaterWispBody", (CharacterMaster master) => BuffWrapperClass<GreaterWispBuffController>.AddBuffComponentToMaster(master) },
                { "ImpBody", (CharacterMaster master) => BuffWrapperClass<ImpBuffController>.AddBuffComponentToMaster(master) },
                { "JellyfishBody", (CharacterMaster master) => BuffWrapperClass<JellyfishBuffController>.AddBuffComponentToMaster(master) },
                { "LemurianBody", (CharacterMaster master) => BuffWrapperClass<LemurianBuffController>.AddBuffComponentToMaster(master) },
                { "LunarGolemBody", (CharacterMaster master) => BuffWrapperClass<LunarGolemBuffController>.AddBuffComponentToMaster(master) },
                { "LunarWispBody", (CharacterMaster master) => BuffWrapperClass<LunarWispBuffController>.AddBuffComponentToMaster(master) },
                { "ParentBody", (CharacterMaster master) => BuffWrapperClass<ParentBuffController>.AddBuffComponentToMaster(master) },
                { "GolemBody", (CharacterMaster master) => BuffWrapperClass<StoneGolemBuffController>.AddBuffComponentToMaster(master) },
                { "NullifierBody", (CharacterMaster master) => BuffWrapperClass<NullifierBuffController>.AddBuffComponentToMaster(master) },
                { "BeetleQueen2Body", (CharacterMaster master) => BuffWrapperClass<BeetleQueenBuffController>.AddBuffComponentToMaster(master) },
                { "GravekeeperBody", (CharacterMaster master) => BuffWrapperClass<GrovetenderBuffController>.AddBuffComponentToMaster(master) },
                { "ClayBossBody", (CharacterMaster master) => BuffWrapperClass<ClayDunestriderBuffController>.AddBuffComponentToMaster(master) },
                { "GrandParentBody", (CharacterMaster master) => BuffWrapperClass<GrandParentBuffController>.AddBuffComponentToMaster(master) },
                { "RoboBallBossBody", (CharacterMaster master) => BuffWrapperClass<SolusControlUnitBuffController>.AddBuffComponentToMaster(master) },
                { "SuperRoboBallBossBody", (CharacterMaster master) => BuffWrapperClass<SolusControlUnitBuffController>.AddBuffComponentToMaster(master) },
                { "MegaConstructBody", (CharacterMaster master) => BuffWrapperClass<XiConstructBuffController>.AddBuffComponentToMaster(master) },
                { "ScavBody", (CharacterMaster master) => BuffWrapperClass<ScavengerBuffController>.AddBuffComponentToMaster(master) },
                { "VoidMegaCrabBody", (CharacterMaster master) => BuffWrapperClass<VoidDevastatorBuffController>.AddBuffComponentToMaster(master) }
            };
        }

    }
}