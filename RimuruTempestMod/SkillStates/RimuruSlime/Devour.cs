using RimuruMod.Modules.Survivors;
using RimuruMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class Devour : BaseMeleeAttack
    {
        private GameObject devour = UnityEngine.Object.Instantiate(Modules.Assets.devour);
        private ParticleSystem mainDevour;
        public override void OnEnter()
        {
            this.hitboxName = "Devour";

            this.damageType = DamageType.BonusToLowHealth;
            this.damageCoefficient = Modules.StaticValues.devourDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            this.bonusForce = new Vector3(0f, -300f, 0f);
            this.baseDuration = 0.5f;
            this.attackStartTime = 0.1f;
            this.attackEndTime = 0.25f;
            this.baseEarlyExitTime = 0.25f;
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
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(devour);
            }
            mainDevour = devour.GetComponent<ParticleSystem>();
            mainDevour.Play();

        }

        public override void Update()
        {
            base.Update();
            devour.transform.position = FindModelChild(this.muzzleString).transform.position;
            devour.transform.rotation = Quaternion.LookRotation(base.characterDirection.forward);            
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

        protected override void SetNextState()
        {

            this.outer.SetNextState(new Devour
            {
            });
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!base.IsKeyDownAuthority())
            {
                mainDevour.Stop();
                if (NetworkServer.active)
                {
                    NetworkServer.Destroy(devour);
                }

            }
        }
    }
}