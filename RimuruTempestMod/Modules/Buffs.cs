using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace RimuruMod.Modules
{
    public static class Buffs
    {
        // spatialmovement armor buff 
        internal static BuffDef SpatialMovementBuff;
        internal static BuffDef CritDebuff;
        internal static BuffDef WetLightningDebuff;
        internal static BuffDef WetDebuff;

        internal static BuffDef StrengthBuff;
        internal static BuffDef FireBuff;
        internal static BuffDef ResistanceBuff;
        internal static BuffDef ShockBuff;
        internal static BuffDef PoisonBuff;
        internal static BuffDef RegenerationBuff;
        internal static BuffDef RegenstackBuff;

        internal static void RegisterBuffs()
        {
            SpatialMovementBuff = AddNewBuff("SpatialMovementBuff", Assets.shieldBuffIcon, Color.cyan, false, false);
            CritDebuff = AddNewBuff("CritDebuff", Assets.critBuffIcon, Color.red, false, true);
            WetLightningDebuff = AddNewBuff("WetLightningDebuff", Assets.lightningBuffIcon, Color.magenta, false, true);
            WetDebuff = AddNewBuff("Wet Debuff", Assets.bleedBuffIcon, Color.cyan, false, true);

            StrengthBuff = AddNewBuff("StrengthBuff", Assets.boostBuffIcon, Color.gray, false, false);
            FireBuff = AddNewBuff("FireBuff", Assets.fireBuffIcon, Color.red, false, false);
            ResistanceBuff = AddNewBuff("ResistanceBuff", Assets.shieldBuffIcon, Color.yellow, false, false);
            ShockBuff = AddNewBuff("ShockBuff", Assets.lightningBuffIcon, Color.yellow, false, false);
            PoisonBuff = AddNewBuff("bleedBuffIcon", Assets.fireBuffIcon, Color.green, false, false);
            RegenerationBuff = AddNewBuff("RegenerationBuff", Assets.healBuffIcon, Color.cyan, false, false);
            RegenstackBuff = AddNewBuff("RegenstackBuff", Assets.healBuffIcon, Color.grey, true, false);
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