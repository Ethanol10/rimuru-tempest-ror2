using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class TransformSlime : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > 0.1f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CharacterBody oldBody = characterBody.master.GetBody();
            var oldHealth = oldBody.healthComponent.health / oldBody.healthComponent.fullHealth;

            characterBody.master.TransformBody("RimuruHumanBody");
            characterMotor.velocity = oldBody.characterMotor.velocity;

            characterBody.healthComponent.health = oldHealth * characterBody.healthComponent.fullHealth;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}