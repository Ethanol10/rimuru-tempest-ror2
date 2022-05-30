﻿using RimuruMod.SkillStates;
using RimuruMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace RimuruMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(SpatialMovement));

            Modules.Content.AddEntityState(typeof(TransformHuman));
            Modules.Content.AddEntityState(typeof(TransformSlime));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}