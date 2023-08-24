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
     Beetle Guard
     Effect: Melee Boost: 1.3 x Melee Damage
     */

    public class BeetleGuardBuffController : RimuruBaseBuffController
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
                body.AddBuff(Buffs.meleeBoostBuff.buffIndex);
            }
            RoR2.Chat.AddMessage("<style=cIsUtility>Melee Boost</style> aquisition successful.");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!body.HasBuff(Buffs.meleeBoostBuff))
            {
                body.ApplyBuff(Buffs.meleeBoostBuff.buffIndex);
            }
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.meleeBoostBuff.buffIndex);
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

