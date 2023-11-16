using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Nullifier
     Effect: Big brain- Reduce CD on 4th hit
     */

    public class NullifierBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.nullifierBigBrainBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Big Brain Skill</style> acquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, RoR2.DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (damageInfo.attacker)
            {
                var attackerBody = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();

                if(attackerBody.HasBuff(Buffs.nullifierBigBrainBuff))
                {
                    int buffcount = attackerBody.GetBuffCount(Buffs.nullifierBigBrainBuffStacks);

                    if(buffcount > StaticValues.nullifierBigBrainThreshold)
                    {
                        attackerBody.skillLocator.DeductCooldownFromAllSkillsServer(1);
                        attackerBody.ApplyBuff(Buffs.nullifierBigBrainBuffStacks.buffIndex, 0);
                    }
                    else
                    {
                        attackerBody.ApplyBuff(Buffs.nullifierBigBrainBuffStacks.buffIndex, buffcount + 1);
                    }
                }
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
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

