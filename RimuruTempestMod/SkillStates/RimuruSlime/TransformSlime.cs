using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class TransformSlime : BaseSkillState
    {
        private CharacterBody oldBody;
        private float oldHealth;

        public override void OnEnter()
        {
            base.OnEnter();
            oldBody = base.characterBody;
            oldHealth = oldBody.healthComponent.health / oldBody.healthComponent.fullHealth;
            Debug.Log(oldHealth);

            characterBody.master.TransformBody("RimuruHumanBody");

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > 0f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            characterMotor.velocity = oldBody.characterMotor.velocity;

            characterBody.healthComponent.health = oldHealth * characterBody.healthComponent.fullHealth;
            Debug.Log(characterBody.healthComponent.health);

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}