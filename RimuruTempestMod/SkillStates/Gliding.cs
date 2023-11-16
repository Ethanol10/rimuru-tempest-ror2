using EntityStates;
using RimuruMod.Content.BuffControllers;
using UnityEngine;

namespace RimuruMod.SkillStates
{
    public class Gliding : BaseState
    {
        private Animator rimuruAnim;
        private static float slowestDescent = -3f;
        private VultureBuffController vultureBuffController; //On the master.
        private bool doNothing;

        public override void OnEnter()
        {
            base.OnEnter();
            rimuruAnim = GetModelAnimator();
            rimuruAnim.SetBool("isGliding", true);
            PlayCrossfade("FullBody, Override", "Sprint1", 0.15f);

            vultureBuffController = characterBody.master.GetComponent<VultureBuffController>();
            doNothing = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                if (vultureBuffController) 
                {
                    if (!vultureBuffController.flightExpired || characterMotor.isGrounded || characterMotor.velocity.y > 0)
                    {
                        doNothing = true;
                    }
                    else 
                    {
                        doNothing = false;
                    }
                }

                if (!doNothing) 
                {
                    float newFallingVelocity = characterMotor.velocity.y / 1.5f;
                    if (newFallingVelocity > slowestDescent)
                    {
                        newFallingVelocity = slowestDescent;
                    }
                    newFallingVelocity = Mathf.MoveTowards(
                        newFallingVelocity,
                        Modules.Config.glideSpeed.Value,
                        Modules.Config.glideAcceleration.Value * Time.fixedDeltaTime
                        * (characterBody.HasBuff(Modules.Buffs.flightBuff) ? Modules.StaticValues.flightAccelerationMultiplier : 1f));
                    characterMotor.velocity = new Vector3(characterMotor.velocity.x, newFallingVelocity, characterMotor.velocity.z);
                }
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
