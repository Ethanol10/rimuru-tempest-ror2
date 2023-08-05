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
     Imp Boss
     Effect: Life Manipulation - 10% life steal
     */

    public class ImpBossBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.lifestealBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Life Manipulation Skill</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.lifestealBuff.buffIndex);
            }
        }

        public void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            orig(self,damageInfo);

            RoR2.CharacterBody attacker = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();

            if (self && attacker)
            {
                if (attacker.HasBuff(Buffs.lifestealBuff))
                {
                    RoR2.ProcChainMask procChainMask = new RoR2.ProcChainMask();
                    procChainMask.mask = 1;

                    attacker.healthComponent.Heal(damageInfo.damage * 0.10f, procChainMask , false);
;                }
            }
        }
    }
}

