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

            characterBody.master.TransformBody("RimuruHumanBody");

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > 0 && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

        }

    }
}