using R2API;
using RimuruMod.Modules;
using RimuruMod.Modules.Survivors;
using RimuruMod.SkillStates.BaseStates;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class Devour : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            this.hitboxName = "Devour";
            this.damageType = DamageType.BonusToLowHealth;

            if (base.characterBody.HasBuff(Modules.Buffs.bleedMeleeBuff))
            {
                damageType |= DamageType.BleedOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.fireBuff))
            {
                damageType |= DamageType.IgniteOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.lightningBuff))
            {
                damageType |= DamageType.Shock5s;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.crippleBuff))
            {
                damageType |= DamageType.CrippleOnHit;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.meleeBoostBuff))
            {
                this.damageCoefficient = Modules.Config.devourDamageCoefficient.Value * 1.3f;
            }
            if (base.characterBody.HasBuff(Modules.Buffs.devourBuff))
            {
                this.hitboxName = "DevourExtended";
            }
            if (base.characterBody.HasBuff(Modules.Buffs.exposeBuff))
            {
                damageType |= DamageType.ApplyMercExpose;
            }
            
            this.procCoefficient = 1f;
            this.damageCoefficient = StaticValues.devourDamageCoefficient;
            this.pushForce = 300f;
            this.bonusForce = new Vector3(0f, -300f, 0f);
            this.baseDuration = 0.8f;
            this.attackStartTime = 0.1f;
            this.attackEndTime = 0.5f;
            this.baseEarlyExitTime = 0.5f;
            this.hitStopDuration = 0.01f;
            this.attackRecoil = 0.2f;
            this.hitHopVelocity = 4f;

            this.swingSoundString = "RimuruSwordSwing";
            this.hitSoundString = "";
            this.muzzleString = "Spine";
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;
            base.OnEnter();
            DamageAPI.AddModdedDamageType(this.attack, Modules.Damage.rimuruDevour);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.hitboxName = "Devour";
        }

        protected override void PlayAttackAnimation()
        {

        }

        protected override void PlaySwingEffect()
        {

        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        //protected override void CheckIfDead(List<HurtBox> hurtboxes) 
        //{
        //    bool playedSound = false;
        //    base.CheckIfDead(hurtboxes);
        //    foreach (HurtBox hurtbox in hurtboxes) 
        //    {
        //        if (hurtbox.healthComponent.health <= 0 && !playedSound) 
        //        {
        //            playedSound = true;
        //            AkSoundEngine.PostEvent("RimuruAnalyse", base.gameObject);
        //        }
        //    }
        //}

        protected override void SetNextState()
        {

            this.outer.SetNextState(new Devour
            {
            });
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}