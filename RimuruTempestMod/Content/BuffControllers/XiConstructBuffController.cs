using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using EffectData = RoR2.EffectData;
using EffectManager = RoR2.EffectManager;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Xi Construct
     Effect: Singular Barrier- every 10 seconds take no damage
     */

    public class XiConstructBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.singularBarrierBuff;
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
                        if (self.body.HasBuff(Buffs.singularBarrierBuff) && !self.body.HasBuff(Buffs.singularBarrierBuffOff) && !flag && damageInfo.damage > 0f && damageInfo.attacker != self.body)
                        {
                            self.body.ApplyBuff(Buffs.singularBarrierBuffOff.buffIndex, 1, StaticValues.singularBarrierInterval);

                            EffectData effectData2 = new EffectData
                            {
                                origin = damageInfo.position,
                                rotation = RoR2.Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                            };
                            EffectManager.SpawnEffect(RoR2.HealthComponent.AssetReferences.bearVoidEffectPrefab, effectData2, true);
                            damageInfo.rejected = true;
                        }
                    }
                }
            }

            orig(self, damageInfo);
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Singular Barrier Skill</style> aquisition successful.");
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
        }
    }
}

