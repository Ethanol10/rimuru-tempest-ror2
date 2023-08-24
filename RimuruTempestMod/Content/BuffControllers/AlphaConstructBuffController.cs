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
     Alpha Construct
     Effect: Hastening - Attack Speed x 1.2
     */

    public class AlphaConstructBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.attackSpeedBuff;
            Hook();
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Bloody Edge Skill</style> aquisition successful.");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public override void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.attackSpeedBuff.buffIndex);
            }
        }

        public void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(Buffs.attackSpeedBuff))
                {
                    self.attackSpeed += StaticValues.speedBuffCoefficient;
                }
            }
        }

        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }


        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }
    }
}

