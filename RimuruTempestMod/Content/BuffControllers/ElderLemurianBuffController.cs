﻿using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Elder Lemurian
     Effect: Strengthen Body - Damage x 1.5
     */

    public class ElderLemurianBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            lifetime = 10f;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.ApplyBuff(Buffs.strengthBuff.buffIndex, 1, -1);
            }
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public void OnDestroy()
        {
            //Unapply StrengthBuff here?
            if (body)
            {
                body.RemoveBuff(Buffs.strengthBuff.buffIndex);
            }
        }

        public void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(RimuruMod.Modules.Buffs.strengthBuff))
                {
                    self.damage *= StaticValues.strengthBuffCoefficient;
                }
            }
        }
    }
}
