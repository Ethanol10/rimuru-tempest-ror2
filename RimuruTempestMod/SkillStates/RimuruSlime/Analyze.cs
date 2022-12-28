using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RimuruMod.Modules.Survivors;

namespace RimuruMod.SkillStates
{
    public class Analyze : BaseSkillState
    {
        public RimuruController Rimurucon;
        public RimuruMasterController Rimurumastercon;
        public HurtBox Target;

        public float duration = 0.1f;
        public override void OnEnter()
        {
            base.OnEnter();
            Rimurucon = base.GetComponent<RimuruController>();
            Rimurumastercon = characterBody.master.gameObject.GetComponent<RimuruMasterController>();
            if (Rimurucon && base.isAuthority)
            {
                Target = Rimurucon.GetTrackingTarget();
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!Target)
            {
                return;
                base.skillLocator.utility.AddOneStock();

            }
            if (Target)
            {
                Target.healthComponent.body.AddTimedBuffAuthority(Modules.Buffs.CritDebuff.buffIndex, Modules.Config.analyseDebuffduration.Value);
                AkSoundEngine.PostEvent(100371988, base.gameObject);
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}