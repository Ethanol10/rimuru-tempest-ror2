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

        internal static BuffDef fireBuff;
        internal static BuffDef resistanceBuff;
        internal static BuffDef lightningBuff;
        internal static BuffDef poisonMeleeBuff;
        internal static BuffDef ultraspeedRegenBuff;
        internal static BuffDef ultraspeedRegenStackBuff;

        internal static void RegisterBuffs()
        {
            SpatialMovementBuff = AddNewBuff("SpatialMovementBuff", Assets.shieldBuffIcon, Color.cyan, false, false);
            CritDebuff = AddNewBuff("CritDebuff", Assets.critBuffIcon, Color.red, false, true);
            WetLightningDebuff = AddNewBuff("WetLightningDebuff", Assets.lightningBuffIcon, Color.magenta, false, true);
            WetDebuff = AddNewBuff("Wet Debuff", Assets.bleedBuffIcon, Color.cyan, false, true);

            strengthBuff = AddNewBuff("Strengthen Body", Assets.boostBuffIcon, Color.gray, false, false);
            speedBuff = AddNewBuff("Acceleration", Assets.boostBuffIcon, Color.blue, false, false);
            attackSpeedBuff = AddNewBuff("Hastening", Assets.boostBuffIcon, Color.yellow, false, false);
            lifestealBuff = AddNewBuff("Life Manipulation", Assets.fireBuffIcon, Color.red, false, false);
            bleedMeleeBuff = AddNewBuff("Bloody Edge", Assets.fireBuffIcon, Color.yellow, false, false);
            meleeBoostBuff = AddNewBuff("Melee Boost", Assets.fireBuffIcon, Color.blue, false, false);
            armourDamageBuff = AddNewBuff("Body Armour", Assets.fireBuffIcon, Color.black, false, false);
            doubleArmourBuff = AddNewBuff("Resistance", Assets.fireBuffIcon, Color.red, false, false);
            jumpHeightBuff = AddNewBuff("Spinglike Limbs", Assets.fireBuffIcon, Color.blue, false, false);
            crippleBuff = AddNewBuff("Crippling Blows", Assets.fireBuffIcon, Color.blue, false, false);
            tripleWaterBladeBuff = AddNewBuff("Triple Waterblade", Assets.fireBuffIcon, Color.yellow, false, false);
            icicleLanceBuff = AddNewBuff("Icicle Lance", Assets.claygooBuffIcon, Color.yellow, false, false);
            barrierBuff = AddNewBuff("Multilayer barrier", Assets.claygooBuffIcon, Color.red, false, false);
            devourBuff = AddNewBuff("Gluttony", Assets.claygooBuffIcon, Color.cyan, false, false);

            fireBuff = AddNewBuff("Fire Manipulation", Assets.fireBuffIcon, Color.red, false, false);
            resistanceBuff = AddNewBuff("Resistance", Assets.shieldBuffIcon, Color.yellow, false, false);
            lightningBuff = AddNewBuff("Paralyzing Breath", Assets.lightningBuffIcon, Color.yellow, false, false);
            poisonMeleeBuff = AddNewBuff("Poisonous attacks", Assets.fireBuffIcon, Color.green, false, false);
            ultraspeedRegenBuff = AddNewBuff("Ultraspeed Regeneration", Assets.healBuffIcon, Color.cyan, false, false);
            ultraspeedRegenStackBuff = AddNewBuff("Ultraspeed Regeneration stacks", Assets.healBuffIcon, Color.grey, true, false);

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
            tarManipBuff = AddNewBuff($"Tar Manipulation- Nearby enemies movespeed and attackspeed are reduced", Assets.claygooBuffIcon, Color.black, false, false);

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