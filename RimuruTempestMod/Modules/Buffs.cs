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
        internal static BuffDef fireMeleeBuff;
        internal static BuffDef meleeBoostBuff;
        internal static BuffDef lightningPulseBuff;
        internal static BuffDef armourDamageBuff;

        //devour buffs
        internal static BuffDef nullifierBigBrainBuff;
        internal static BuffDef nullifierBigBrainBuffStacks;

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
            fireMeleeBuff = AddNewBuff("Fire Manipulation", Assets.fireBuffIcon, Color.black, false, false);
            armourDamageBuff = AddNewBuff("", Assets.fireBuffIcon, Color.black, false, false);

            fireBuff = AddNewBuff("Fire Manipulation", Assets.fireBuffIcon, Color.red, false, false);
            resistanceBuff = AddNewBuff("Resistance", Assets.shieldBuffIcon, Color.yellow, false, false);
            lightningBuff = AddNewBuff("Lightning Manipulation", Assets.lightningBuffIcon, Color.yellow, false, false);
            poisonMeleeBuff = AddNewBuff("Poisonous attacks", Assets.fireBuffIcon, Color.green, false, false);
            ultraspeedRegenBuff = AddNewBuff("Ultraspeed Regeneration", Assets.healBuffIcon, Color.cyan, false, false);
            ultraspeedRegenStackBuff = AddNewBuff("Ultraspeed Regeneration stacks", Assets.healBuffIcon, Color.grey, true, false);

            //devour buffs
            nullifierBigBrainBuff = AddNewBuff($"Big Brain Buff- Reduce CD on every {StaticValues.nullifierBigBrainThreshold}th hit", Assets.healBuffIcon, Color.grey, true, false);
            nullifierBigBrainBuffStacks = AddNewBuff($"Big Brain Buff stacks", Assets.healBuffIcon, Color.grey, true, false);
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