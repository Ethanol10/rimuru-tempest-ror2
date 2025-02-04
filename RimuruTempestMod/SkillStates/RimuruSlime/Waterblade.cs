﻿using EntityStates;
using RimuruMod.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RimuruMod.SkillStates
{
    public class Waterblade : BaseSkillState
    {
        public static float damageCoefficient = Modules.Config.waterbladeDamageCoefficient.Value;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.65f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Waterblade.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                AkSoundEngine.PostEvent(2654748154, base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    if (base.characterBody.HasBuff(Modules.Buffs.ConductivityBuff))
                    {
                        Modules.Projectiles.waterbladeProjectile.GetComponent<ProjectileDamage>().damageType.damageType |= DamageType.Shock5s;
                    }
                    if (base.characterBody.HasBuff(Modules.Buffs.TarProjBuff))
                    {
                        Modules.Projectiles.waterbladeProjectile.GetComponent<ProjectileDamage>().damageType.damageType |= DamageType.ClayGoo;
                    }
                    if (base.characterBody.HasBuff(Modules.Buffs.CrippleProjBuff))
                    {
                        Modules.Projectiles.waterbladeProjectile.GetComponent<ProjectileDamage>().damageType.damageType |= DamageType.CrippleOnHit;
                    }


                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.waterbladeProjectile, 
                        aimRay.origin, 
                        Util.QuaternionSafeLookRotation(aimRay.direction), 
                        base.gameObject,
                        Waterblade.damageCoefficient * this.damageStat, 
                        0f, 
                        base.RollCrit(), 
                        DamageColorIndex.Default, 
                        null,
                        Waterblade.throwForce);
                    if (base.characterBody.HasBuff(Buffs.tripleWaterBladeBuff))
                    {
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.waterbladeProjectile,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z + 1)),
                        base.gameObject,
                        Waterblade.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        Waterblade.throwForce);
                        
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.waterbladeProjectile,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z - 1)),
                        base.gameObject,
                        Waterblade.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        Waterblade.throwForce);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}