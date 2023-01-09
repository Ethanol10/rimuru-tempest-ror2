using System;

namespace RimuruMod.Modules
{
    internal static class StaticValues
    {
        //passives
        //strength
        internal const float strengthBuffCoefficient = 1.5f;

        //ultraspeed
        internal const float ultraspeedRegenCoefficient = 0.5f;
        internal const float ultraspeedHealthThreshold = 0.1f;
        internal const int ultraspeedBuffStacks = 5;

        //lightning
        internal const float lightningPulseTimer = 2f;


        //melee attacks
        internal const float swordDamageCoefficient = 2f;
        internal const float devourDamageCoefficient = 1f;

        //blacklightning
        internal const float blacklightningtotalDuration = 4f;
        internal const float blacklightningDamageCoefficient = 1f;
        internal const float blacklightningProcCoefficient = 0.5f;
        internal const float blacklightningRadius = 10f;

        //spatial movement
        internal const int spatialmovementbuffDuration = 3;
        internal const float spatialmovementbuffArmor = 50f;

        //analyze
        internal const int analyzedebuffDuration = 6;

        //Waterblade parameters
        internal const float waterbladeDamageCoefficient = 2.0f;
        internal const float waterbladeForce = 100f;
        internal const float waterbladeProjectileLifetime = 2.0f;
        internal const float waterbladeProjectileSpeed = 20f;

        //Wet debuff Params
        internal const float wetDebuffLen = 6.0f;

        //lemurian fire buff
        internal const float lemurianfireDamageCoefficient = 4f;
        internal const float lemurianfireProcCoefficient = 1f;
        internal const float lemurianfireRadius = 15f;
    }
}