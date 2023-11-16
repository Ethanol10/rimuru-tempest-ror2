using EntityStates;
using RimuruMod.Modules.Survivors;
using RimuruMod.SkillStates.BaseStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimuruMod.SkillStates
{
    public class DashAttackExit : BaseMeleeAttack
    {
        protected int baseAttackAmount = 3;
        protected int attackAmount = 0;

        protected float attackTimer = 0f;
        protected float attackStopwatch = 0f;

        protected float stopHoptrigger = 0.35f;

        public override void OnEnter()
        {
            this.hitboxName = "Sword";

            this.damageType = DamageType.Generic;
            if (base.characterBody.HasBuff(Modules.Buffs.fireBuff))
            {
                damageType |= DamageType.IgniteOnHit;
            }
            this.damageCoefficient = Modules.Config.swordDamageCoefficient.Value;
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            this.bonusForce = new Vector3(0f, -500f, 0f);
            this.baseDuration = 1.3f;
            this.attackStartTime = 0.25f;
            this.attackEndTime = 0.65f;
            this.baseEarlyExitTime = 0.70f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 7f;

            this.swingSoundString = "RimuruSword";
            this.hitSoundString = "";
            this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            float multiplier = base.attackSpeedStat >= 1f ? base.attackSpeedStat : 1f;
            attackAmount = (int)(baseAttackAmount * multiplier);


            base.OnEnter();


            attackTimer = (duration * attackEndTime) - (duration * attackStartTime) / (float)attackAmount;
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayCrossfade("FullBody, Override", "OutOfDashAttack", "Slash.playbackRate", duration, 0.05f);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            Util.PlaySound(this.hitSoundString, base.gameObject);

            if (!this.inHitPause && this.hitStopDuration > 0f)
            {
                this.storedVelocity = base.characterMotor.velocity;
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                this.hitPauseTimer = this.hitStopDuration / this.attackSpeedStat;
                this.inHitPause = true;
            }
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            this.outer.SetNextState(new SlashCombo
            {
                swingIndex = index
            });
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.stopwatch >= this.duration * attackStartTime && base.isAuthority && this.stopwatch <= this.duration * stopHoptrigger) 
            {
                characterMotor.Motor.ForceUnground();
                characterMotor.velocity = new Vector3(characterMotor.velocity.x, Mathf.Max(characterMotor.velocity.y, Modules.Config.dashAttackHop.Value), characterMotor.velocity.z);
            }

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        protected override void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayAttackSpeedSound(this.swingSoundString, base.gameObject, this.attackSpeedStat);

                if (base.isAuthority)
                {
                    this.PlaySwingEffect();
                    base.AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
                }


                if (base.characterBody.HasBuff(Modules.Buffs.icicleLanceBuff))
                {

                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.icicleLanceProjectile,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
                    base.gameObject,
                    damageCoefficient * this.damageStat,
                    0f,
                    base.RollCrit(),
                    DamageColorIndex.Default,
                    null,
                    -1f);
                }
            }

            attackStopwatch += Time.fixedDeltaTime;

            if (attackStopwatch > attackTimer) 
            {
                attackStopwatch = 0f;
                CreateNewAttack();
            }

            if (base.isAuthority)
            {
                List<HurtBox> hurtboxes = new List<HurtBox>();
                bool result = this.attack.Fire(hurtboxes);
                if (result)
                {
                    this.OnHitEnemyAuthority();
                    this.CheckIfDead(hurtboxes);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}