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
     Parent
     Effect: Multilayer Barrier - Gain maximum barrier
     */

    public class ParentBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;
        public float timer;

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
                body.AddBuff(Buffs.barrierBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Multilayer Barrier</style> aquisition successful.");
        }
        
        public void Update()
        {
            timer += Time.deltaTime;
            if (body)
            {
                if (body.HasBuff(Buffs.barrierBuff))
                {
                    if (timer > 3f)
                    {
                        body.healthComponent.AddBarrierAuthority(body.maxHealth * 0.1f);
                        timer = 0;
                    }
                }
            }
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.barrierBuff.buffIndex);
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

