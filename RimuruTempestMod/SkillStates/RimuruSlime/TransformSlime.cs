using EntityStates;
using RimuruTempestMod.Content.BuffControllers;
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
            oldBody = base.characterBody;
            oldHealth = oldBody.healthComponent.health;
            characterBody.master.TransformBody("RimuruHumanBody");
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
            RimuruBaseBuffController[] array = characterBody.master.GetComponents<RimuruBaseBuffController>();
            foreach (RimuruBaseBuffController controller in array)
            {
                controller.UpdateBody();
            }
            characterBody.master.GetBody().healthComponent.health = oldHealth;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}