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
     Void Jailer
     Effect: Gravity pulse- hit enemies pulse, pulling enemies towards them
     */

    public class VoidJailerBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.gravityPulseBuff;
        }

        public void Hook()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, RoR2.DamageInfo damageInfo, GameObject victim)
        {

            orig.Invoke(self, damageInfo, victim);

            var attackerBody = damageInfo.attacker.GetComponent<RoR2.CharacterBody>();

            if (attackerBody && attackerBody.HasBuff(Buffs.gravityPulseBuff))
            {
                var victimBody = victim.GetComponent<RoR2.CharacterBody>();

                if (damageInfo.damage > 0 && (damageInfo.damageType & DamageType.DoT) != DamageType.DoT && damageInfo.procCoefficient > 0f)
                {
                    new GravityPulsePullRequest(attackerBody.masterObjectId, victimBody.corePosition, Vector3.up, StaticValues.gravityPulseRange, 0f, damageInfo.damage, 360f, true).Send(NetworkDestination.Clients);

                }

            }

        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Gravity Pulse Skill</style> acquisition successful.");
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

        public override void OnDestroy()
        {
            base.OnDestroy();

            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }
    }
}

