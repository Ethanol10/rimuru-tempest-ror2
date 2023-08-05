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

        //ultraspeed
        internal const float ultraspeedRegenCoefficient = 0.5f;
        internal const float ultraspeedHealthThreshold = 0.1f;
        internal const int ultraspeedBuffStacks = 5;

        //lightning
        internal const float lightningPulseTimer = 2f;


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
        internal const float hermitMortarRadius = 10f;
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
            rimDic = new Dictionary<string, Func<CharacterMaster, RimuruBaseBuffController>>();

            rimDic.Add("MinorConstructBody", (CharacterMaster master) => BuffWrapperClass<AlphaConstructBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("MinorConstructOnKillBody", (CharacterMaster master) => BuffWrapperClass<AlphaConstructBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("BeetleBody", (CharacterMaster master) => BuffWrapperClass<BeetleBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("FlyingVerminBody", (CharacterMaster master) => BuffWrapperClass<FlyingVerminBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("VerminBody", (CharacterMaster master) => BuffWrapperClass<BlindVerminBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("GupBody", (CharacterMaster master) => BuffWrapperClass<GupBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("GipBody", (CharacterMaster master) => BuffWrapperClass<GupBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("GeepBody", (CharacterMaster master) => BuffWrapperClass<GupBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("HermitCrabBody", (CharacterMaster master) => BuffWrapperClass<HermitCrabBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("AcidLarvaBody", (CharacterMaster master) => BuffWrapperClass<AcidLarvaBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("WispBody", IndicatorType.PASSIVE);
            rimDic.Add("LunarExploderBody", (CharacterMaster master) => BuffWrapperClass<LunarExploderBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("MiniMushroomBody", IndicatorType.PASSIVE);
            //rimDic.Add("RoboBallMiniBody", IndicatorType.PASSIVE);
            //rimDic.Add("RoboBallGreenBuddyBody", IndicatorType.PASSIVE);
            //rimDic.Add("RoboBallRedBuddyBody", IndicatorType.PASSIVE);
            rimDic.Add("VoidBarnacleBody", (CharacterMaster master) => BuffWrapperClass<VoidBarnacleBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VoidJailerBody", IndicatorType.PASSIVE);
            rimDic.Add("ImpBossBody", (CharacterMaster master) => BuffWrapperClass<ImpBossBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("TitanBody", IndicatorType.PASSIVE);
            //rimDic.Add("TitanGoldBody", IndicatorType.PASSIVE);
            rimDic.Add("VagrantBody", (CharacterMaster master) => BuffWrapperClass<WanderingVagrantBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("MagmaWormBody", (CharacterMaster master) => BuffWrapperClass<MagmaWormBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ElectricWormBody", IndicatorType.PASSIVE);
            //rimDic.Add("CaptainBody", IndicatorType.PASSIVE);
            //rimDic.Add("CommandoBody", IndicatorType.PASSIVE);
            //rimDic.Add("CrocoBody", IndicatorType.PASSIVE);
            //rimDic.Add("LoaderBody", IndicatorType.PASSIVE);
            rimDic.Add("VultureBody", (CharacterMaster master) => BuffWrapperClass<VultureBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("BeetleGuardBody", (CharacterMaster master) => BuffWrapperClass<BeetleGuardBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("BisonBody", IndicatorType.ACTIVE);
            //rimDic.Add("BellBody", IndicatorType.ACTIVE);
            //rimDic.Add("ClayGrenadierBody", IndicatorType.ACTIVE);
            //rimDic.Add("ClayBruiserBody", IndicatorType.ACTIVE);
            rimDic.Add("LemurianBruiserBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GreaterWispBody", IndicatorType.ACTIVE);
            rimDic.Add("ImpBody", (CharacterMaster master) => BuffWrapperClass<ImpBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("JellyfishBody", IndicatorType.ACTIVE);
            //rimDic.Add("LemurianBody", IndicatorType.ACTIVE);
            //rimDic.Add("LunarGolemBody", IndicatorType.ACTIVE);
            rimDic.Add("LunarWispBody", (CharacterMaster master) => BuffWrapperClass<LunarWispBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("ParentBody", (CharacterMaster master) => BuffWrapperClass<ParentBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("GolemBody", (CharacterMaster master) => BuffWrapperClass<StoneGolemBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("NullifierBody", (CharacterMaster master) => BuffWrapperClass<NullifierBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("BeetleQueen2Body", (CharacterMaster master) => BuffWrapperClass<BeetleQueenBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("GravekeeperBody", (CharacterMaster master) => BuffWrapperClass<GrovetenderBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ClayBossBody", IndicatorType.ACTIVE);
            //rimDic.Add("GrandParentBody", IndicatorType.ACTIVE);
            //rimDic.Add("RoboBallBossBody", IndicatorType.ACTIVE);
            //rimDic.Add("SuperRoboBallBossBody", IndicatorType.ACTIVE);
            //rimDic.Add("MegaConstructBody", IndicatorType.ACTIVE);
            rimDic.Add("ScavBody", (CharacterMaster master) => BuffWrapperClass<ScavengerBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VoidMegaCrabBody", IndicatorType.ACTIVE);
        }

    }
}