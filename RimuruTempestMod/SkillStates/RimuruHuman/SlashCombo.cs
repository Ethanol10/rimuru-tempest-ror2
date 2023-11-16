using RimuruMod.Modules;
using RimuruMod.Modules.Survivors;
using RimuruMod.SkillStates.BaseStates;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace RimuruMod.SkillStates
{
    public class SlashCombo : BaseMeleeAttack
    {
        public RimuruController Rimurucon;
        public RimuruMasterController Rimurumastercon;
        public HurtBox Target;
        public override void OnEnter()
        {
            this.hitboxName = "Sword";

            this.damageType = DamageType.Generic;
            if (base.characterBody.HasBuff(Modules.Buffs.bleedMeleeBuff))
            {
                damageType |= DamageType.BleedOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.fireBuff))
            {
                damageType |= DamageType.IgniteOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.poisonMeleeBuff))
            {
                damageType |= DamageType.BlightOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.lightningBuff))
            {
                damageType |= DamageType.Shock5s;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.crippleBuff))
            {
                damageType |= DamageType.CrippleOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.exposeBuff))
            {
                damageType |= DamageType.ApplyMercExpose;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.meleeBoostBuff))
            {
                this.damageCoefficient = Modules.Config.devourDamageCoefficient.Value * 1.3f;
            }
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            this.bonusForce = new Vector3(0f, -300f, 0f);
            this.baseDuration = 0.63f;
            this.attackStartTime = 0.3f;
            this.attackEndTime = 0.5f;
            this.baseEarlyExitTime = 0.75f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 7f;

            this.swingSoundString = "RimuruSword";
            this.hitSoundString = "";
            this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            Rimurucon = base.GetComponent<RimuruController>();
            Rimurumastercon = characterBody.master.gameObject.GetComponent<RimuruMasterController>();
            if (Rimurucon && base.isAuthority)
            {
                Target = Rimurucon.GetTrackingTarget();
            }
            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            if (Target)
            {
                float num = 10f;
                if (!base.isGrounded)
                {
                    num = 7f;
                }
                float num2 = Vector3.Distance(base.transform.position, Target.transform.position);
                if (num2 >= num)
                {
                    this.outer.SetNextState(new DashAttack
                    {

                    });
                }
                else
                {
                    this.outer.SetNextState(new SlashCombo
                    {
                        swingIndex = index
                    });

                }
            }
            else if (!Target)
            {
                this.outer.SetNextState(new SlashCombo
                {
                    swingIndex = index
                });
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}