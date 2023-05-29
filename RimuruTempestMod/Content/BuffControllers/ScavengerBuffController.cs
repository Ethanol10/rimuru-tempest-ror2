using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruMod;
using RimuruTempestMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Scavenger
     Effect: Creation- duplicate a random item on boss kill
     */

    public class ScavengerBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

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
                body.AddBuff(Buffs.scavengerReplicationBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Replication Skill</style> aquisition successful.");
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

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.scavengerReplicationBuff.buffIndex);
            }
        }

        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            body.AddBuff(Buffs.scavengerReplicationBuff.buffIndex);
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }
    }
}

