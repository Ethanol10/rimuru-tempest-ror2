using EntityStates;
using RoR2;

namespace RimuruMod.SkillStates.Rimuru
{
    internal class RimuruCharacterMain : GenericCharacterMain
    {
        private EntityStateMachine weaponStateMachine;

        //Ripped from mage lmao
        public override void OnEnter()
        {
            base.OnEnter();
            this.weaponStateMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Weapon");
        }

        public override void ProcessJump()
        {
            base.ProcessJump();
            if (this.hasCharacterMotor && this.hasInputBank && base.isAuthority)
            {
                bool CheckJumpingHold = base.inputBank.jump.down && (base.characterMotor.velocity.y < 0f || base.characterBody.HasBuff(Modules.Buffs.flightBuff)) && !base.characterMotor.isGrounded;
                bool flag = this.weaponStateMachine.state.GetType() == typeof(Gliding);

                if (CheckJumpingHold && !flag)
                {
                    this.weaponStateMachine.SetNextState(new Gliding());
                }
                if (!CheckJumpingHold && flag)
                {
                    this.weaponStateMachine.SetNextState(new Idle());
                }
            }
        }

        public override void OnExit()
        {
            if (base.isAuthority && this.weaponStateMachine)
            {
                this.weaponStateMachine.SetNextState(new Idle());
            }
            base.OnExit();
        }
    }
}
