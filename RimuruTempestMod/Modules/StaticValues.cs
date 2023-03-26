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
        //strength
        internal const float strengthBuffCoefficient = 1.5f;

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


        public static Dictionary<string, Func<CharacterMaster, RimuruBaseBuffController>> rimDic;

        class BuffWrapperClass<T> where T: RimuruBaseBuffController
        {
            public static T AddBuffComponentToMaster(CharacterMaster master) 
            {
                T returnObj = master.gameObject.GetComponent<T>();
                return returnObj ? returnObj : master.gameObject.AddComponent<T>();
            }
        }

        public static void LoadDictionary()
        {
            // CharacterMaster is input, RimuruBaseBuffController is output. Instantiating dictionary at beginning.
            rimDic = new Dictionary<string, Func<CharacterMaster, RimuruBaseBuffController>>();


            //rimDic.Add("MinorConstructBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("MinorConstructOnKillBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("BeetleBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("FlyingVerminBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VerminBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GupBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GipBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GeepBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GupBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("HermitCrabBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("AcidLarvaBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("WispBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("LunarExploderBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("MiniMushroomBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("RoboBallMiniBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("RoboBallGreenBuddyBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("RoboBallRedBuddyBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VoidBarnacleBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VoidJailerBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ImpBossBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("TitanBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("TitanGoldBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VagrantBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("MagmaWormBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ElectricWormBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("CaptainBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("CrocoBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("LoaderBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VultureBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("BeetleGuardBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("BisonBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("BellBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ClayGrenadierBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ClayBruiserBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            rimDic.Add("LemurianBruiserBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GreaterWispBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ImpBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("JellyfishBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("LemurianBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("LunarGolemBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("LunarWispBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ParentBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GolemBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("NullifierBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("BeetleQueen2Body", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GravekeeperBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("ClayBossBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("GrandParentBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("RoboBallBossBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("SuperRoboBallBossBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("MegaConstructBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
            //rimDic.Add("VoidMegaCrabBody", (CharacterMaster master) => BuffWrapperClass<ElderLemurianBuffController>.AddBuffComponentToMaster(master));
        }

    }
}