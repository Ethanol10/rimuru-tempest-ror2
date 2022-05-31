using EntityStates;
using RimuruMod.Modules.Survivors;
using RimuruMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RimuruMod.SkillStates
{
    public class RimuruHumanPrimary : BaseSkillState
    {
        public RimuruController Rimurucon;
        public RimuruMasterController Rimurumastercon;
        public HurtBox Target;
        public override void OnEnter()
        {
            Rimurucon = base.GetComponent<RimuruController>();
            Rimurumastercon = characterBody.master.gameObject.GetComponent<RimuruMasterController>();
            if (Rimurucon && base.isAuthority)
            {
                Target = Rimurucon.GetTrackingTarget();
            }

            if(Target)
            {
                float num = 10f;
                if (!base.isGrounded)
                {
                    num = 7f;
                }
                float num2 = Vector3.Distance(base.transform.position, Target.transform.position);
                if (num2 >= num)
                {
                    this.outer.SetNextState(new DashAttack
                    {

                    });
                }
                else
                {
                    this.outer.SetNextState(new SlashCombo
                    {

                    });

                }
            }
            else if (!Target)
            {
                this.outer.SetNextState(new SlashCombo
                {

                });
            }

            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}