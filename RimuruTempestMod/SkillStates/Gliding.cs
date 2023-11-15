using EntityStates;
using UnityEngine;

namespace RimuruMod.SkillStates
{
    public class Gliding : BaseState
    {
        private Animator rimuruAnim;
        private static float slowestDescent = -3f;

        public override void OnEnter()
        {
            base.OnEnter();
            rimuruAnim = GetModelAnimator();
            rimuruAnim.SetBool("isGliding", true);
            PlayCrossfade("FullBody, Override", "Sprint1", 0.15f);

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                float newFallingVelocity = characterMotor.velocity.y / 1.5f;
                if (newFallingVelocity > slowestDescent)
                {
                    newFallingVelocity = slowestDescent;
                }
                newFallingVelocity = Mathf.MoveTowards(newFallingVelocity, Modules.Config.glideSpeed.Value, Modules.Config.glideAcceleration.Value * Time.fixedDeltaTime);
                characterMotor.velocity = new Vector3(characterMotor.velocity.x, newFallingVelocity, characterMotor.velocity.z);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            rimuruAnim.SetBool("isGliding", false);
            base.PlayAnimation("FullBody, Override", "BufferEmpty");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
