using EntityStates;
using R2API.Networking.Interfaces;
using RimuruMod.Content.BuffControllers;
using RimuruMod.Modules.Networking;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class TransformSlime : BaseSkillState
    {
        public CharacterBody oldBody;
        public float oldHealth;

        public override void OnEnter()
        {
            base.OnEnter();

            oldHealth = characterBody.healthComponent.health;
            new TransformBody(characterBody.master.netId, oldHealth, (int)TransformBody.TargetBody.HUMAN).Send(R2API.Networking.NetworkDestination.Clients);
            AkSoundEngine.PostEvent("RimuruTransform", base.gameObject);
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
            new UpdateControllers(characterBody.master.netId, oldHealth).Send(R2API.Networking.NetworkDestination.Clients);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}