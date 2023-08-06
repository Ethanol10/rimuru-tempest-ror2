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
     Blind Vermin
     Effect: Acceleration - Movement Speed x 1.2
     */

    public class BlindVerminBuffController : RimuruBaseBuffController
    {
        


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
                body.AddBuff(Buffs.speedBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Acceleration Skill</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!body.HasBuff(Buffs.speedBuff))
            {
                body.ApplyBuff(Buffs.speedBuff.buffIndex);
            }
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.speedBuff.buffIndex);
            }
        }

        public void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(Buffs.speedBuff))
                {
                    self.moveSpeed *= StaticValues.speedBuffCoefficient;
                }
            }
        }
    }
}

