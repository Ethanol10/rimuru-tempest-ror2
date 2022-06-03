using RimuruMod.SkillStates;
using RimuruMod.SkillStates.BaseStates;

namespace RimuruMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(SpawnState));

            Modules.Content.AddEntityState(typeof(Devour));

            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(DashAttack));
            Modules.Content.AddEntityState(typeof(DashAttackExit));
            Modules.Content.AddEntityState(typeof(RimuruHumanPrimary));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(BlackLightning));

            Modules.Content.AddEntityState(typeof(SpatialMovement));

            Modules.Content.AddEntityState(typeof(TransformHuman));
            Modules.Content.AddEntityState(typeof(TransformSlime));

            Modules.Content.AddEntityState(typeof(Waterblade));
        }
    }
}