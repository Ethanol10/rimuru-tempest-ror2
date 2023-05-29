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
     Stone Golem
     Effect: Body Armour - +Damage = Armour * 0.1
     */

    public class StoneGolemBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = true;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.armourDamageBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Body Armour</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.armourDamageBuff.buffIndex);
            }
        }

        public void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(Buffs.armourDamageBuff))
                {
                    self.damage += self.armor * 0.1f;
                }
            }
        }
    }
}
