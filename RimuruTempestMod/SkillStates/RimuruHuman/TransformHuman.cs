using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class TransformHuman : BaseSkillState
    {
        public CharacterBody oldBody;
        public float oldHealth;

        public override void OnEnter()
        {
            base.OnEnter();
            oldBody = base.characterBody;
            oldHealth = oldBody.healthComponent.health;
            characterBody.master.TransformBody("RimuruSlimeBody");
            AkSoundEngine.PostEvent(1422622395, base.gameObject);
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
            characterBody.master.GetBody().healthComponent.health = oldHealth;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}