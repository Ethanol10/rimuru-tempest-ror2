using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using IL.RoR2;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Bronzong
     Effect: Spiked body- when you get hit you deal damage around you
     */

    public class BronzongBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.spikedBodyBuff;
        }

        public void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {

            if (self)
            {
                if (damageInfo.attacker)
                {
                    if (self.body)
                    {
                        bool flag = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
                        if (self.body.HasBuff(Buffs.spikedBodyBuff) && !flag && damageInfo.damage > 0f && damageInfo.attacker != self.body)
                        {
                            RoR2.BlastAttack blastAttack = new RoR2.BlastAttack();
                            
                            blastAttack.radius = StaticValues.spikedBodyRange;
                            blastAttack.procCoefficient = 0.5f;
                            blastAttack.position = self.transform.position;
                            blastAttack.attacker = self.gameObject;
                            blastAttack.crit = RoR2.Util.CheckRoll(self.body.crit, self.body.master);
                            blastAttack.baseDamage = self.body.damage;
                            blastAttack.falloffModel = RoR2.BlastAttack.FalloffModel.None;
                            blastAttack.baseForce = 100f;
                            blastAttack.teamIndex = RoR2.TeamComponent.GetObjectTeam(self.body.gameObject);
                            blastAttack.damageType = DamageType.Generic;
                            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


                            blastAttack.Fire();


                            RoR2.EffectManager.SpawnEffect(Assets.GupSpikeEffect, new RoR2.EffectData
                            {
                                origin = self.transform.position,
                                scale = StaticValues.spikedBodyRange,
                                rotation = Quaternion.LookRotation(self.transform.position)

                            }, true);
                        }
                    }
                }
            }
            
            orig(self, damageInfo);
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Bloody Edge Skill</style> aquisition successful.");
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
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }
    }
}

