using BepInEx.Configuration;
using RiskOfOptions.Options;
using RiskOfOptions;
using UnityEngine;
using RiskOfOptions.OptionConfigs;

namespace RimuruMod.Modules
{
    public static class Config
    {
        //Melee Attacks - 01
        public static ConfigEntry<float> swordDamageCoefficient;
        public static ConfigEntry<float> devourDamageCoefficient;

        //Black Lightning - 02
        public static ConfigEntry<float> blackLightningTotalDuration;
        public static ConfigEntry<float> blackLightningDamageCoefficient;
        public static ConfigEntry<float> blackLightningProcCoefficient;
        public static ConfigEntry<float> blackLightningRadius;
        public static ConfigEntry<float> blackLightningCooldown;

        //Spatial Movement - 03
        public static ConfigEntry<float> spatialMovementBuffDuration;
        public static ConfigEntry<float> spatialMovementCooldown;

        //Analyse - 04
        public static ConfigEntry<bool> doubleInsteadOfCrit;
        public static ConfigEntry<float> analyseDebuffduration;
        public static ConfigEntry<float> analyseCooldown;

        //Waterblade - 05
        public static ConfigEntry<float> waterbladeDamageCoefficient;
        public static ConfigEntry<float> waterbladeWetDebuffDuration;
        public static ConfigEntry<float> waterbladeCooldown;

        //Add config for all damage coefficients.
        /*
        //Waterblade parameters 05
        internal const float waterbladeForce = 100f;
        internal const float waterbladeProjectileLifetime = 2.0f;
        internal const float waterbladeProjectileSpeed = 20f;
        //lemurian fire buff 07
        internal const float lemurianfireDamageCoefficient = 4f;
        internal const float lemurianfireProcCoefficient = 1f;
        internal const float lemurianfireRadius = 15f;
         */

        public static void ReadConfig()
        {
            //Melee - 01
            swordDamageCoefficient = RimuruPlugin.instance.Config.Bind<float>("01 - Sword/Devour", "01 - Sword Damage Coefficient", 2.0f, "Determines the damage coefficient for Rimuru's Human Form, Sword.");
            devourDamageCoefficient = RimuruPlugin.instance.Config.Bind<float>("01 - Sword/Devour", "02 - Devour Damage Coefficient", 1.0f, "Determines the damage coefficient for Rimuru's Slime form, Devour");

            //Black Lightning - 02
            blackLightningTotalDuration = RimuruPlugin.instance.Config.Bind<float>("02 - Black Lightning", "01 - Black Lightning Total Duration", 4f, "Determines how long Rimuru should fire black lightning for.");
            blackLightningDamageCoefficient = RimuruPlugin.instance.Config.Bind<float>("02 - Black Lightning", "02 - Black Lightning Damage Coefficient", 1f, "Determines the damage coefficient for each tick of Black Lightning.");
            blackLightningProcCoefficient = RimuruPlugin.instance.Config.Bind<float>("02 - Black Lightning", "03 - Black Lightning Proc Coefficient", 0.5f, "Determines how often a status effect should be applied when using black lightning.");
            blackLightningRadius = RimuruPlugin.instance.Config.Bind<float>("02 - Black Lightning", "04 - Black Lightning Radius", 10.0f, "Determines the blast radius when black lightning hits an enemy affected by 'Wet'.");
            blackLightningCooldown = RimuruPlugin.instance.Config.Bind<float>("02 - Black Lightning", "05 - Black Lightning Cooldown", 6f, "Determines the cooldown for Black Lightning, Needs a restart to apply.");

            //Spatial Movement - 03
            spatialMovementBuffDuration = RimuruPlugin.instance.Config.Bind<float>("03 - Spatial Movement", "01 - Spatial Movement Buff Duration", 3f, "Determines how long the buff should last after performing Spatial Movement.");
            spatialMovementCooldown = RimuruPlugin.instance.Config.Bind<float>("03 - Spatial Movement", "02 - Spatial Movement Cooldown", 10f, "Determines the cooldown for Spatial movement, Needs a restart to apply.");

            //Analyse - 04
            doubleInsteadOfCrit = RimuruPlugin.instance.Config.Bind<bool>("04 - Analyse", "01 - Double Damage instead of Crit", false, "Determines if the applied analyse debuff should double damage instead of force a crit.");
            analyseDebuffduration = RimuruPlugin.instance.Config.Bind<float>("04 - Analyse", "02 - Analyse Debuff Duration", 6f, "Determines the duration of the buff applied to the enemy when analyse is used.");
            analyseCooldown = RimuruPlugin.instance.Config.Bind<float>("04 - Analyse", "03 - Analyse Cooldown", 6f, "Determines the cooldown for Analyse, Needs a restart to apply.");

            //Waterblade - 05
            waterbladeDamageCoefficient = RimuruPlugin.instance.Config.Bind<float>("05 - Waterblade", "01 - Waterblade Damage Coefficient", 2.0f, "Determines the damage coefficient on waterblade");
            waterbladeWetDebuffDuration = RimuruPlugin.instance.Config.Bind<float>("05 - Waterblade", "02 - Waterblade wet debuff duration", 6f, "Determines how long the Wet debuff should last.");
            waterbladeCooldown = RimuruPlugin.instance.Config.Bind<float>("05 - Waterblade", "03 - Waterblade Cooldown", 1f, "Determines the cooldown for Waterblade, Needs a restart to apply.");
        }

        public static void SetupRiskOfOptions() 
        {
            //Melee - 01
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    swordDamageCoefficient,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.1f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    devourDamageCoefficient,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.1f
                    }
                ));

            //Black Lightning - 02
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    blackLightningTotalDuration,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.25f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    blackLightningDamageCoefficient,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.1f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    blackLightningProcCoefficient,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.1f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    blackLightningRadius,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 1f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    blackLightningCooldown,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.25f
                    }
                ));

            //Spatial Movement - 03
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    spatialMovementBuffDuration,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.5f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    spatialMovementCooldown,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.1f
                    }
                ));

            //Analyse - 04
            ModSettingsManager.AddOption(new CheckBoxOption(
                doubleInsteadOfCrit));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    analyseDebuffduration,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.5f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    analyseCooldown,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.5f
                    }
                ));

            //Waterblade - 05
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    waterbladeDamageCoefficient,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.5f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    waterbladeWetDebuffDuration,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.5f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    waterbladeCooldown,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 100f,
                        increment = 0.1f
                    }
                ));
        }

        // this helper automatically makes config entries for disabling survivors
        public static ConfigEntry<bool> CharacterEnableConfig(string characterName, string description = "Set to false to disable this character", bool enabledDefault = true) {

            return RimuruPlugin.instance.Config.Bind<bool>("General",
                                                          "Enable " + characterName,
                                                          enabledDefault,
                                                          description);
        }
    }
}