﻿using RoR2;
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
        internal static BuffDef wetDebuff;

        internal static void RegisterBuffs()
        {
            SpatialMovementBuff = AddNewBuff("SpatialMovementBuff", Assets.shieldBuffIcon, Color.cyan, false, false);
            CritDebuff = AddNewBuff("CritDebuff", Assets.critBuffIcon, Color.red, false, true);
            WetLightningDebuff = AddNewBuff("WetLightningDebuff", Assets.lightningBuffIcon, Color.magenta, false, true);
            wetDebuff = AddNewBuff("Wet Debuff", Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdBleeding.asset").WaitForCompletion().iconSprite, Color.cyan, false, true);
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