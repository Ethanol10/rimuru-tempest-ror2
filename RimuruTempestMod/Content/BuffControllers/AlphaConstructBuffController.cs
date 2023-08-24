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
            Hook();
            isPermaBuff = false;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.bleedMeleeBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Hastening</style> aquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!body.HasBuff(Buffs.bleedMeleeBuff))
            {
                body.ApplyBuff(Buffs.bleedMeleeBuff.buffIndex);
            }
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.bleedMeleeBuff.buffIndex);
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

