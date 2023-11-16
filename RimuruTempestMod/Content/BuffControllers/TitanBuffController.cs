using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Titan
     Effect: Refraction- attacks chain to nearby enemies
     */

    public class TitanBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.refractionBuff;
        }

        public void Hook()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, RoR2.DamageInfo damageInfo, GameObject victim)
        {

            var attacker = damageInfo.attacker;
            if (attacker)
            {
                var attackerbody = attacker.GetComponent<RoR2.CharacterBody>();
                var victimBody = victim.GetComponent<RoR2.CharacterBody>();

                if(attackerbody.HasBuff(Buffs.refractionBuff))
                {
                    if (damageInfo.damage > 0 && (damageInfo.damageType & DamageType.DoT) != DamageType.DoT && damageInfo.procCoefficient > 0f)
                    {
                        new OrbDamageRequest(victimBody.masterObjectId, damageInfo.damage, attackerbody.masterObjectId).Send(NetworkDestination.Clients);
                    }
                }
            }

            orig.Invoke(self, damageInfo, victim);
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Bloody Edge Skill</style> acquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
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
        public override void OnDestroy()
        {
            base.OnDestroy();
            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }
    }
}

