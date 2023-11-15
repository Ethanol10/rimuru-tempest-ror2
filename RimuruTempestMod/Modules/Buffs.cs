using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;
using IL.RoR2.Achievements.Bandit2;

namespace RimuruMod.Modules
{
    public static class Buffs
    {
        // spatialmovement armor buff 
        internal static BuffDef SpatialMovementBuff;
        internal static BuffDef CritDebuff;
        internal static BuffDef WetLightningDebuff;
        internal static BuffDef WetDebuff;

        internal static BuffDef strengthBuff;
        internal static BuffDef speedBuff;
        internal static BuffDef attackSpeedBuff;
        internal static BuffDef lifestealBuff;
        internal static BuffDef bleedMeleeBuff;
        internal static BuffDef meleeBoostBuff;
        internal static BuffDef lightningPulseBuff;
        internal static BuffDef armourDamageBuff;
        internal static BuffDef doubleArmourBuff;
        internal static BuffDef jumpHeightBuff;
        internal static BuffDef crippleBuff;
        internal static BuffDef tripleWaterBladeBuff;
        internal static BuffDef icicleLanceBuff;
        internal static BuffDef barrierBuff;
        internal static BuffDef devourBuff;

        //devour buffs
        internal static BuffDef nullifierBigBrainBuff;
        internal static BuffDef nullifierBigBrainBuffStacks;
        internal static BuffDef scavengerReplicationBuff;
        internal static BuffDef lunarExploderLuckManipulationBuff;
        internal static BuffDef hermitMortarBuff;
        internal static BuffDef aoeBufferBuff;
        internal static BuffDef gravManipulationBuff;
        internal static BuffDef flightBuff;
        internal static BuffDef flameBodyBuff;
        internal static BuffDef tarManipBuff;
        internal static BuffDef tarManipDebuff;
        internal static BuffDef reparationBuff;
        internal static BuffDef reparationBuffStacks;
        internal static BuffDef dashBuff;
        internal static BuffDef tarringBuff;
        internal static BuffDef hyperRegenBuff;
        internal static BuffDef gravityPulseBuff;
        internal static BuffDef spikedBodyBuff;
        internal static BuffDef cleanserBuff;
        internal static BuffDef refractionBuff;
        internal static BuffDef singularBarrierBuff;
        internal static BuffDef singularBarrierBuffOff;
        internal static BuffDef reverseGravManipBuff;

        internal static BuffDef fireBuff;
        internal static BuffDef resistanceBuff;
        internal static BuffDef lightningBuff;
        internal static BuffDef poisonMeleeBuff;
        internal static BuffDef ultraspeedRegenBuff;
        internal static BuffDef ultraspeedRegenStackBuff;
        internal static BuffDef lightningDamageBoostBuff;
        internal static BuffDef lightningProcBoostBuff;
        internal static BuffDef ConductivityBuff;
        internal static BuffDef exposeBuff;
        internal static BuffDef TarProjBuff;
        internal static BuffDef CrippleProjBuff;
        internal static void RegisterBuffs()
        {
            SpatialMovementBuff = AddNewBuff($"SpatialMovementBuff", Assets.shieldBuffIcon, Color.cyan, false, false);
            CritDebuff = AddNewBuff($"CritDebuff", Assets.critBuffIcon, Color.red, false, true);
            WetLightningDebuff = AddNewBuff($"WetLightningDebuff", Assets.lightningBuffIcon, Color.magenta, false, true);
            WetDebuff = AddNewBuff($"Wet Debuff", Assets.bleedBuffIcon, Color.cyan, false, true);

            strengthBuff = AddNewBuff($"Strengthen Body- Increase your Damage by {StaticValues.strengthBuffCoefficient}x", Assets.boostBuffIcon, Color.gray, false, false);
            speedBuff = AddNewBuff($"Acceleration- Increase your Movespeed by {StaticValues.speedBuffCoefficient}x", Assets.speedBuffIcon, Color.blue, false, false);
            attackSpeedBuff = AddNewBuff($"Hastening- Increase your Attackspeed by {StaticValues.attackSpeedBuffCoefficient}x", Assets.attackspeedBuffIcon, Color.yellow, false, false);
            lifestealBuff = AddNewBuff($"Life Manipulation- Gain {StaticValues.lifestealBuffCoefficient * 100f}% lifesteal", Assets.healBuffIcon, Color.red, false, false);
            bleedMeleeBuff = AddNewBuff($"Bloody Edge- Melee attacks apply bleed", Assets.bleedBuffIcon, Color.red, false, false);
            meleeBoostBuff = AddNewBuff($"Melee Boost- Melee attacks deal 1.3x", Assets.fireBuffIcon, Color.blue, false, false);
            armourDamageBuff = AddNewBuff($"Body Armour- Gain {StaticValues.bodyArmorCoefficient} of your Damage as Armor", Assets.shieldBuffIcon, Color.black, false, false);
            doubleArmourBuff = AddNewBuff($"Resistance- Double your armor", Assets.shieldBuffIcon, Color.red, false, false);
            jumpHeightBuff = AddNewBuff($"Spinglike Limbs- Increase your jump height", Assets.jumpBuffIcon, Color.blue, false, false);
            crippleBuff = AddNewBuff($"Crippling Blows- Melee attacks apply cripple", Assets.crippleBuffIcon, Color.blue, false, false);
            tripleWaterBladeBuff = AddNewBuff($"Triple Waterblade- Shoot 3 Waterblades instead of 1!", Assets.tarBuffIcon, Color.cyan, false, false);
            icicleLanceBuff = AddNewBuff($"Icicle Lance- Melee attacks shoot an ice projectile", Assets.lightningBuffIcon, Color.blue, false, false);
            barrierBuff = AddNewBuff($"Multilayer barrier", Assets.claygooBuffIcon, Color.red, false, false);
            devourBuff = AddNewBuff($"Gluttony", Assets.claygooBuffIcon, Color.cyan, false, false);

            fireBuff = AddNewBuff($"Fire Manipulation", Assets.fireBuffIcon, Color.red, false, false);
            resistanceBuff = AddNewBuff($"Resistance", Assets.shieldBuffIcon, Color.yellow, false, false);
            lightningBuff = AddNewBuff($"Paralyzing Breath", Assets.lightningBuffIcon, Color.yellow, false, false);
            poisonMeleeBuff = AddNewBuff($"Poisonous attacks", Assets.fireBuffIcon, Color.green, false, false);
            ultraspeedRegenBuff = AddNewBuff($"Ultraspeed Regeneration", Assets.healBuffIcon, Color.cyan, false, false);
            ultraspeedRegenStackBuff = AddNewBuff($"Ultraspeed Regeneration stacks", Assets.healBuffIcon, Color.grey, true, false);

            //devour buffs
            nullifierBigBrainBuff = AddNewBuff($"Big Brain Buff- Reduce CD on every {StaticValues.nullifierBigBrainThreshold}th hit", Assets.noCooldownBuffIcon, Color.magenta, false, false);
            nullifierBigBrainBuffStacks = AddNewBuff($"Big Brain Buff stacks", Assets.noCooldownBuffIcon, Color.grey, true, false);
            scavengerReplicationBuff = AddNewBuff($"Creation Buff- Create a random item on boss kill", Assets.bearVoidReadyBuffIcon, Color.yellow, false, false);
            lunarExploderLuckManipulationBuff = AddNewBuff($"Luck Manipulation Buff- Get 1 luck", Assets.shurikenBuffIcon, Color.blue, false, false);
            hermitMortarBuff = AddNewBuff($"Mortaring Buff- Standing still shoots projectiles at nearby enemies", Assets.mortarBuffIcon, Color.grey, false, false);
            aoeBufferBuff = AddNewBuff($"AOE Buffer Buff- Analyze now analyzes all nearby enemies", Assets.boostBuffIcon, Color.white, false, false);
            gravManipulationBuff = AddNewBuff($"Gravity Manipulation- Nearby enemies are pulled down to the ground", Assets.claygooBuffIcon, Color.magenta, false, false);
            flightBuff = AddNewBuff($"Flight- Hold space to fly, up to 3 seconds of height gain then glide", Assets.jumpBuffIcon, Color.cyan, false, false);
            flameBodyBuff = AddNewBuff($"Flame Body- Burn nearby enemies", Assets.strongerBurnIcon, Color.red, false, false);
            tarManipBuff = AddNewBuff($"Tar Manipulation- Nearby enemies movespeed and attackspeed are reduced by {(1 - StaticValues.tarManipCoefficient)* 100f}%", Assets.claygooBuffIcon, Color.black, false, false);
            tarManipDebuff = AddNewBuff($"Tar Debuff", Assets.claygooBuffIcon, Color.black, false, true);
            reparationBuff = AddNewBuff($"Reparation- heal 50% of the recent damage you've taken after 5 seconds", Assets.medkitBuffIcon, Color.green, false, false);
            reparationBuffStacks = AddNewBuff($"Reparation Stacks", Assets.medkitBuffIcon, Color.black, true, false);
            dashBuff = AddNewBuff($"Dash- pressing sprint dashes", Assets.sprintBuffIcon, Color.yellow, false, false);
            tarringBuff = AddNewBuff($"Tarring- Fire Tar at the closest enemies every second", Assets.mortarBuffIcon, Color.black, false, false);
            hyperRegenBuff = AddNewBuff($"Hyper regeneration- Heal {StaticValues.hyperRegenCoefficient * 100f}% of max HP per second", Assets.healBuffIcon, Color.yellow, false, false);
            gravityPulseBuff = AddNewBuff($"Gravity pulse- hit enemies pulse, pulling enemies towards them", Assets.ruinDebuffIcon, Color.magenta, false, false);
            spikedBodyBuff = AddNewBuff($"Spiked body- when you get hit you deal damage around you", Assets.spikeBuffIcon, Color.yellow, false, false);
            cleanserBuff = AddNewBuff($"Cleanser- Cleanse yourself every {StaticValues.cleanserInterval} seconds", Assets.alphashieldoffBuffIcon, Color.red, false, false);
            refractionBuff = AddNewBuff($"Refraction- attacks chain to nearby enemies", Assets.lunarRootIcon, Color.white, false, false);
            singularBarrierBuff = AddNewBuff($"Singular Barrier- every {StaticValues.singularBarrierInterval} seconds take no damage", Assets.shieldBuffIcon, Color.white, false, false);
            singularBarrierBuffOff = AddNewBuff($"Singular Barrier deactivated", Assets.shieldBuffIcon, Color.black, false, false);
            reverseGravManipBuff = AddNewBuff($"Reverse Gravity Manipulation- enemies get knocked up every {StaticValues.reverseGravManipInterval} seconds", Assets.resonanceBuffIcon, Color.cyan, false, false);
            lightningPulseBuff = AddNewBuff($"Lightning Pulse - Shock nearby enemies every 2 seconds", Assets.resonanceBuffIcon, Color.cyan, false, false);
            lightningDamageBoostBuff = AddNewBuff($"Spark - Lightning deals 3x damage", Assets.resonanceBuffIcon, Color.cyan, false, false);
            lightningProcBoostBuff = AddNewBuff($"Black Flare - Lightning deals 4x proc rate", Assets.resonanceBuffIcon, Color.cyan, false, false);
            ConductivityBuff = AddNewBuff($"Conductivity - Waterblade additionally deals shock damage ", Assets.resonanceBuffIcon, Color.cyan, false, false);
            exposeBuff = AddNewBuff($"Void-touched blows - Melee attacks apply expose ", Assets.resonanceBuffIcon, Color.cyan, false, false);
            CrippleProjBuff = AddNewBuff($"Crippling Waterblade - Waterblade additionally deals cripple damage ", Assets.resonanceBuffIcon, Color.cyan, false, false);
            TarProjBuff = AddNewBuff($"Tarred Waterblade - Waterblade additionally deals tar damage ", Assets.resonanceBuffIcon, Color.cyan, false, false);

        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}