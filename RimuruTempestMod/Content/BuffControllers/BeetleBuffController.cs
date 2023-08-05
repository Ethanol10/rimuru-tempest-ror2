using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Beetle
     Effect: Resistance: Double Armour
     */

    public class BeetleBuffController : RimuruBaseBuffController
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
                body.AddBuff(Buffs.doubleArmourBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Resistance</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.doubleArmourBuff.buffIndex);
            }
        }

        public void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(Buffs.doubleArmourBuff))
                {
                    self.armor += self.armor;
                }
            }
        }

        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            body.AddBuff(Buffs.nullifierBigBrainBuff.buffIndex);
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }
    }
}

