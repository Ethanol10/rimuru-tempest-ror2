using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class TransformSlime : BaseSkillState
    {
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

           
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= duration)
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