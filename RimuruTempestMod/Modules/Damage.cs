using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace RimuruMod.Modules
{
    public static class Damage
    {
        internal static DamageAPI.ModdedDamageType rimuruDevour;

        internal static void SetupModdedDamage()
        {
            rimuruDevour = DamageAPI.ReserveDamageType();
        }
    }
}
