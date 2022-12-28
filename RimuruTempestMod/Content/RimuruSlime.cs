using BepInEx.Configuration;
using EntityStates;
using RimuruMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimuruMod.Modules.Survivors
{
    internal class RimuruSlime : SurvivorBase
    {
        public override string bodyName => "RimuruSlime";

        public const string RIMURU_PREFIX = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => RIMURU_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "RimuruSlimeBody",
            bodyNameToken = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME",
            subtitleNameToken = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texRimuruIcon"),
            bodyColor = Color.cyan,

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 1f,
            armor = 10f,
            damage = 10f,
            healthGrowth = 20f,
            jumpCount = 2,
            moveSpeed = 7f,
        };

        internal static Material RimuruSlimeMat = Modules.Assets.mainAssetBundle.LoadAsset<Material>("RimuruHumanMat");
        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = RimuruSlimeMat,
                }
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter(bool isHidden)
        {
            base.InitializeCharacter(isHidden);
            bodyPrefab.AddComponent<RimuruController>();
            EntityStateMachine rimuruEntityStateMachine = bodyPrefab.GetComponent<EntityStateMachine>();
            rimuruEntityStateMachine.initialStateType = new SerializableEntityStateType(typeof(SkillStates.BaseStates.SpawnState));
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            //example of how to create a hitbox
            Transform hitboxTransform = childLocator.FindChild("DevourHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Devour");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = RimuruPlugin.DEVELOPER_PREFIX;

            #region Primary
            //Creates a skilldef for a typical primary 
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(prefix + "_RIMURU_BODY_PRIMARY_DEVOUR_NAME",
                                                                                      prefix + "_RIMURU_BODY_PRIMARY_DEVOUR_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("devourIcon"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.Devour)),
                                                                                      "Weapon",
                                                                                      true));


            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region Secondary
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_RIMURU_BODY_SECONDARY_WATERBLADE_NAME",
                skillNameToken = prefix + "_RIMURU_BODY_SECONDARY_WATERBLADE_NAME",
                skillDescriptionToken = prefix + "_RIMURU_BODY_SECONDARY_WATERBLADE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("waterbladeIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Waterblade)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Modules.Config.waterbladeCooldown.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef);
            #endregion

            #region Utility
            SkillDef analyzeSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_RIMURU_BODY_UTILITY_ANALYZE_NAME",
                skillNameToken = prefix + "_RIMURU_BODY_UTILITY_ANALYZE_NAME",
                skillDescriptionToken = prefix + "_RIMURU_BODY_UTILITY_ANALYZE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("analyseIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Analyze)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Modules.Config.analyseCooldown.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, analyzeSkillDef);
            #endregion

            #region Special
            SkillDef transformSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_RIMURU_BODY_SPECIAL_TRANSFORMSLIME_NAME",
                skillNameToken = prefix + "_RIMURU_BODY_SPECIAL_TRANSFORMSLIME_NAME",
                skillDescriptionToken = prefix + "_RIMURU_BODY_SPECIAL_TRANSFORMSLIME_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texTransformToHuman"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TransformSlime)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, transformSkillDef);
            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            Material defaultMat = Modules.Materials.CreateHopooMaterial("RimuruSlimeMat");
            Material emptyMat = Modules.Assets.mainAssetBundle.LoadAsset<Material>("EmptyMat");
            CharacterModel.RendererInfo[] defaultRendererInfo = SkinRendererInfos(defaultRenderers, new Material[] {
                defaultMat,
            });
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("skinIcon"),
                defaultRendererInfo,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                //place your mesh replacements here
                //unnecessary if you don't have multiple skins
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruSlimeMesh"),
                    renderer = defaultRenderers[0].renderer
                },
            };
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            //Material masteryMat = Modules.Materials.CreateHopooMaterial("matRimuruAlt");
            //CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            //{
            //    masteryMat,
            //    masteryMat,
            //    masteryMat,
            //    masteryMat
            //});

            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_MASTERY_SKIN_NAME",
            //    Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    masteryRendererInfos,
            //    mainRenderer,
            //    model,
            //    masterySkinUnlockableDef);

            //masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            //{
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshRimuruSwordAlt"),
            //        renderer = defaultRenderers[0].renderer
            //    },
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshRimuruAlt"),
            //        renderer = defaultRenderers[2].renderer
            //    }
            //};

            //skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];

            return newRendererInfos;
        }
    }
}