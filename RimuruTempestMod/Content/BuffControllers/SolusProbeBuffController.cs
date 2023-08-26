using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using IL.RoR2;
using RimuruMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Solus Probe
     Reparation- on take damage, heal 50% of the damage you take after 5 seconds
     */

    public class SolusProbeBuffController : RimuruBaseBuffController
    {
        private float timer;
        private float healAmount;

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.reparationBuff;
        }

        public void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {

            if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.GetComponent<RoR2.CharacterBody>())
            {
                if(self.body.HasBuff(Buffs.reparationBuff))
                {
                    int dmgAmount = self.body.GetBuffCount(Buffs.reparationBuffStacks);
                    dmgAmount += (int)(damageInfo.damage * StaticValues.reparationCoefficient);

                    self.body.ApplyBuff(Buffs.reparationBuffStacks.buffIndex, dmgAmount); 
                }
            }

            orig.Invoke(self, damageInfo);
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Reparation Skill</style> aquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (body.HasBuff(Buffs.reparationBuffStacks))
            {
                timer += Time.fixedDeltaTime;
                if(timer > StaticValues.reparationTimer)
                {
                    timer = 0f;
                    healAmount = body.GetBuffCount(Buffs.reparationBuffStacks);
                    new HealNetworkRequest(body.masterObjectId, healAmount).Send(NetworkDestination.Clients);
                    body.ApplyBuff(Buffs.reparationBuffStacks.buffIndex, 0);
                }
            }
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
            body.ApplyBuff(Buffs.reparationBuffStacks.buffIndex, 0);
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }
    }
}

