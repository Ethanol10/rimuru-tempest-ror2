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
     Flying Vermin aka Blind Pest
     Effect: AOE buffer- analyze analzyes multiple enimes
     */

    public class FlyingVerminBuffController : RimuruBaseBuffController
    {
        


        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.aoeBufferBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>AOE Buffer Skill</style> aquisition successful.");
        }

        public void Hook()
        {

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!body.HasBuff(Buffs.aoeBufferBuff))
            {
                body.ApplyBuff(Buffs.aoeBufferBuff.buffIndex);
            }
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.aoeBufferBuff.buffIndex);
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

