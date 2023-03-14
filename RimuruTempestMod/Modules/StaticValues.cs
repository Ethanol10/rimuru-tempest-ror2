﻿using RimuruMod.Modules.Survivors;
using RimuruTempestMod.Content.BuffControllers;
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


        public static Dictionary<string, RimuruBaseBuffController> rimDic;

        public static void LoadDictionary()
        {
            ElderLemurianBuffController elderLemCon = new ElderLemurianBuffController();

            rimDic = new Dictionary<string, RimuruBaseBuffController>();
            rimDic.Add("MinorConstructBody", elderLemCon);
            rimDic.Add("MinorConstructOnKillBody", IndicatorType.PASSIVE);
            rimDic.Add("BeetleBody", IndicatorType.PASSIVE);
            rimDic.Add("FlyingVerminBody", IndicatorType.PASSIVE);
            rimDic.Add("VerminBody", IndicatorType.PASSIVE);
            rimDic.Add("GupBody", IndicatorType.PASSIVE);
            rimDic.Add("GipBody", IndicatorType.PASSIVE);
            rimDic.Add("GeepBody", IndicatorType.PASSIVE);
            rimDic.Add("HermitCrabBody", IndicatorType.PASSIVE);
            rimDic.Add("AcidLarvaBody", IndicatorType.PASSIVE);
            rimDic.Add("WispBody", IndicatorType.PASSIVE);
            rimDic.Add("LunarExploderBody", IndicatorType.PASSIVE);
            rimDic.Add("MiniMushroomBody", IndicatorType.PASSIVE);
            rimDic.Add("RoboBallMiniBody", IndicatorType.PASSIVE);
            rimDic.Add("RoboBallGreenBuddyBody", IndicatorType.PASSIVE);
            rimDic.Add("RoboBallRedBuddyBody", IndicatorType.PASSIVE);
            rimDic.Add("VoidBarnacleBody", IndicatorType.PASSIVE);
            rimDic.Add("VoidJailerBody", IndicatorType.PASSIVE);
            rimDic.Add("ImpBossBody", IndicatorType.PASSIVE);
            rimDic.Add("TitanBody", IndicatorType.PASSIVE);
            rimDic.Add("TitanGoldBody", IndicatorType.PASSIVE);
            rimDic.Add("VagrantBody", IndicatorType.PASSIVE);
            rimDic.Add("MagmaWormBody", IndicatorType.PASSIVE);
            rimDic.Add("ElectricWormBody", IndicatorType.PASSIVE);
            rimDic.Add("CaptainBody", IndicatorType.PASSIVE);
            rimDic.Add("CommandoBody", IndicatorType.PASSIVE);
            rimDic.Add("CrocoBody", IndicatorType.PASSIVE);
            rimDic.Add("LoaderBody", IndicatorType.PASSIVE);

            rimDic.Add("VultureBody", IndicatorType.ACTIVE);
            rimDic.Add("BeetleGuardBody", IndicatorType.ACTIVE);
            rimDic.Add("BisonBody", IndicatorType.ACTIVE);
            rimDic.Add("BellBody", IndicatorType.ACTIVE);
            rimDic.Add("ClayGrenadierBody", IndicatorType.ACTIVE);
            rimDic.Add("ClayBruiserBody", IndicatorType.ACTIVE);
            rimDic.Add("LemurianBruiserBody", IndicatorType.ACTIVE);
            rimDic.Add("GreaterWispBody", IndicatorType.ACTIVE);
            rimDic.Add("ImpBody", IndicatorType.ACTIVE);
            rimDic.Add("JellyfishBody", IndicatorType.ACTIVE);
            rimDic.Add("LemurianBody", IndicatorType.ACTIVE);
            rimDic.Add("LunarGolemBody", IndicatorType.ACTIVE);
            rimDic.Add("LunarWispBody", IndicatorType.ACTIVE);
            rimDic.Add("ParentBody", IndicatorType.ACTIVE);
            rimDic.Add("GolemBody", IndicatorType.ACTIVE);
            rimDic.Add("NullifierBody", IndicatorType.ACTIVE);
            rimDic.Add("BeetleQueen2Body", IndicatorType.ACTIVE);
            rimDic.Add("GravekeeperBody", IndicatorType.ACTIVE);
            rimDic.Add("ClayBossBody", IndicatorType.ACTIVE);
            rimDic.Add("GrandParentBody", IndicatorType.ACTIVE);
            rimDic.Add("RoboBallBossBody", IndicatorType.ACTIVE);
            rimDic.Add("SuperRoboBallBossBody", IndicatorType.ACTIVE);
            rimDic.Add("MegaConstructBody", IndicatorType.ACTIVE);
            rimDic.Add("VoidMegaCrabBody", IndicatorType.ACTIVE);
        }

    }
}