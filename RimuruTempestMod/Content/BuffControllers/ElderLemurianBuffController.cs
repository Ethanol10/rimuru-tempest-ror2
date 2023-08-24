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
     Elder Lemurian
     Effect: Strengthen Body - Damage x 1.5
     */

    public class ElderLemurianBuffController : RimuruBaseBuffController
    {
        


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
                body.ApplyBuff(Buffs.strengthBuff.buffIndex, 1, -1);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Strengthen Body Skill</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!body.HasBuff(Buffs.strengthBuff))
            {
                body.ApplyBuff(Buffs.strengthBuff.buffIndex);
            }
        }
        public override void OnDestroy()
        {
            //Unapply StrengthBuff here?
            if (body)
            {
                body.RemoveBuff(Buffs.strengthBuff.buffIndex);
            }
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
        }

        public void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(Buffs.strengthBuff))
                {
                    self.damage *= StaticValues.strengthBuffCoefficient;
                }
            }
        }
        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }
    }
}

