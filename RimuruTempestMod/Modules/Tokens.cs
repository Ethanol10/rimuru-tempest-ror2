using R2API;
using System;

namespace RimuruMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Rimuru
            string prefix = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_";
            string humanPrefix = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUHUMAN_BODY_";
            string slimePrefix = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_";

            string desc = "Rimuru is a form-changing character that alternates between utility and damage.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > His sword attack can dash towards enemies." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > He has elemental interactions when an enemy is wet, shocked, or ignited." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Aim to switch between his forms to maximise his damage output." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Different enemies grant different passive buffs from Slime form's Devour." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, continuing on his comfortable life.";
            string outroFailure = "..and so he went on, making sure to put his hard drive into the bath.";

            LanguageAPI.Add(prefix + "NAME", "Rimuru Tempest");
            LanguageAPI.Add(slimePrefix + "NAME", "Rimuru Tempest");
            LanguageAPI.Add(humanPrefix + "NAME", "Rimuru Tempest");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Great Demon Lord");
            LanguageAPI.Add(prefix + "LORE", "I am the slime Rimuru Tempest. I'm not a bad slime!");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region HumanSkills
            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Magicule Properties");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Hitting burning enemies with shock damage stuns and creates an explosion. " + Environment.NewLine +
                "Hitting wet enemies with fire damage ignites nearby enemies and removes the wet debuff. " + Environment.NewLine +
                "Hitting wet enemies with shock damage shocks nearby enemies.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword of Tempest");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * Modules.Config.swordDamageCoefficient.Value}% damage</style>, dashing to distant enemies.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BLACKLIGHTNING_NAME", "Black Lightning");
            LanguageAPI.Add(prefix + "SECONDARY_BLACKLIGHTNING_DESCRIPTION", Helpers.agilePrefix + $"Shock all enemies in front of you for <style=cIsDamage>{100f * Config.blackLightningDamageCoefficient.Value}% damage per tick </style>" +
                $"for {Modules.Config.blackLightningTotalDuration.Value} seconds.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_SPATIALMOVEMENT_NAME", "Spatial Movement");
            LanguageAPI.Add(prefix + "UTILITY_SPATIALMOVEMENT_DESCRIPTION", Helpers.agilePrefix + $"Hold to aim and release to teleport, gaining <style=cIsUtility>300 armor</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TRANSFORMHUMAN_NAME", "Transform");
            LanguageAPI.Add(prefix + "SPECIAL_TRANSFORMHUMAN_DESCRIPTION", $"Transform into your slime form.");
            #endregion
            #endregion

            #region SlimeSkills

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_DEVOUR_NAME", "Devour");
            LanguageAPI.Add(prefix + "PRIMARY_DEVOUR_DESCRIPTION", Helpers.agilePrefix + "<style=cIsUtility>Slayer.</style> " + $"Devour all enemies in front of you for <style=cIsDamage>{100f * Modules.Config.devourDamageCoefficient.Value}% damage</style>. " +
                $"Gain buffs depending on which enemy type is killed.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_WATERBLADE_NAME", "Waterblade");
            LanguageAPI.Add(prefix + "SECONDARY_WATERBLADE_DESCRIPTION", Helpers.agilePrefix + $"Fire a Waterblade for <style=cIsDamage>{100f * Config.waterbladeDamageCoefficient.Value}% damage</style>, applying a wet debuff on them.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ANALYZE_NAME", "Analyze");
            LanguageAPI.Add(prefix + "UTILITY_ANALYZE_DESCRIPTION", Helpers.agilePrefix + $"Analyze the target, gaining <style=cIsDamage>guaranteed crits</style> on them for {(int)Modules.Config.analyseDebuffduration.Value} seconds.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TRANSFORMSLIME_NAME", "Transform");
            LanguageAPI.Add(prefix + "SPECIAL_TRANSFORMSLIME_DESCRIPTION", $"Transform into your human form.");
            #endregion
            #endregion
            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Rimuru: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Rimuru, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Rimuru: Mastery");
            #endregion
            #endregion
        }
    }
}