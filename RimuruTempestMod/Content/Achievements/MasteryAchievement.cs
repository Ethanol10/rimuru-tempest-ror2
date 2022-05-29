using RoR2;
using System;
using UnityEngine;

namespace RimuruMod.Modules.Achievements
{
    internal class MasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_MASTERY";
        //the name of the sprite in your bundle
        public override string AchievementSpriteName => "texMasteryAchievement";
        //the token of your character's unlock achievement if you have one
        public override string PrerequisiteUnlockableIdentifier => RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_UNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => "RimuruBody";
        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}