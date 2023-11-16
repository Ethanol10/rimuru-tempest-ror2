using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruMod;
using RimuruMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Scavenger
     Effect: Creation- duplicate a random item on boss kill
     */

    public class ScavengerBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.scavengerReplicationBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Replication Skill Skill</style> acquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, RoR2.GlobalEventManager self, RoR2.DamageReport damageReport)
        {
            orig(self,damageReport);

            if (damageReport.attackerBody.HasBuff(Buffs.scavengerReplicationBuff))
            {
                if(damageReport.victimBody.isBoss)
                {
                    new ItemDropNetworked(damageReport.attackerBody.masterObjectId).Send(NetworkDestination.Clients);
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
            On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
        }

        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            base.ActiveBuffEffect();
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }
    }
}

