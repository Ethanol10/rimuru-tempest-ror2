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
    internal class RimuruHuman : SurvivorBase
    {
        public override string bodyName => "RimuruHuman";

        public const string RIMURU_PREFIX = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => RIMURU_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "RimuruHumanBody",
            bodyNameToken = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUHUMAN_BODY_NAME",
            subtitleNameToken = RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUHUMAN_BODY_SUBTITLE",

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

        //internal static Material RimuruHumanMat = Modules.Assets.mainAssetBundle.LoadAsset<Material>("RimuruHumanMat");
        internal static Material RimuruHumanMat = Modules.Materials.CreateHopooMaterial("RimuruHumanMat");
        internal static Material RimuruHumanEmptyMat = Modules.Assets.mainAssetBundle.LoadAsset<Material>("EmptyMat");
        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "Wing",
                    material = RimuruHumanMat,
                },
                new CustomRendererInfo
                {
                    childName = "Sword",
                    material = RimuruHumanMat,
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = RimuruHumanMat,
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
            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = RimuruPlugin.DEVELOPER_PREFIX;

            #region Passive
            SkillLocator skillloc = bodyPrefab.GetComponent<SkillLocator>();
            skillloc.passiveSkill.enabled = true;
            skillloc.passiveSkill.skillNameToken = prefix + "_RIMURU_BODY_PASSIVE_NAME";
            skillloc.passiveSkill.skillDescriptionToken = prefix + "_RIMURU_BODY_PASSIVE_DESCRIPTION";
            skillloc.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("magiculePropertiesIcon");
            #endregion

            #region Primary
            //Creates a skilldef for a typical primary 
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(prefix + "_RIMURU_BODY_PRIMARY_SLASH_NAME",
                                                                                      prefix + "_RIMURU_BODY_PRIMARY_SLASH_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSwordSwing"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.RimuruHumanPrimary)),
                                                                                      "Weapon",
                                                                                      true));


            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region Secondary
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_RIMURU_BODY_SECONDARY_BLACKLIGHTNING_NAME",
                skillNameToken = prefix + "_RIMURU_BODY_SECONDARY_BLACKLIGHTNING_NAME",
                skillDescriptionToken = prefix + "_RIMURU_BODY_SECONDARY_BLACKLIGHTNING_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("blackLightningIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BlackLightning)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Modules.Config.blackLightningCooldown.Value,
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
            SkillDef spatialSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_RIMURU_BODY_UTILITY_SPATIALMOVEMENT_NAME",
                skillNameToken = prefix + "_RIMURU_BODY_UTILITY_SPATIALMOVEMENT_NAME",
                skillDescriptionToken = prefix + "_RIMURU_BODY_UTILITY_SPATIALMOVEMENT_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("spatialTeleportIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SpatialMovement)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Modules.Config.spatialMovementCooldown.Value,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, spatialSkillDef);
            #endregion

            #region Special
            SkillDef transformSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_RIMURU_BODY_SPECIAL_TRANSFORMHUMAN_NAME",
                skillNameToken = prefix + "_RIMURU_BODY_SPECIAL_TRANSFORMHUMAN_NAME",
                skillDescriptionToken = prefix + "_RIMURU_BODY_SPECIAL_TRANSFORMHUMAN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texTransformToSlime"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TransformHuman)),
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
            Material defaultMat = Modules.Materials.CreateHopooMaterial("RimuruHumanMat");
            Material emptyMat = Modules.Assets.mainAssetBundle.LoadAsset<Material>("EmptyMat");
            CharacterModel.RendererInfo[] defaultRendererInfo = SkinRendererInfos(defaultRenderers, new Material[] {
                defaultMat,
                defaultMat,
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
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruWingMesh"),
                    renderer = defaultRendererInfo[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruSwordMesh"),
                    renderer = defaultRendererInfo[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruHumanMesh"),
                    renderer = defaultRendererInfo[2].renderer
                }
            };

            skins.Add(defaultSkin);
            #endregion

            //masked skin, removed since mask is wack
            //#region maskedSkin
            //CharacterModel.RendererInfo[] maskedrendererInfos = SkinRendererInfos(defaultRenderers, new Material[] {
            //    defaultMat,
            //    defaultMat,
            //    defaultMat,
            //    defaultMat,
            //});
            //SkinDef maskedSkin = Modules.Skins.CreateSkinDef(RimuruPlugin.DEVELOPER_PREFIX + "_RIMURU_BODY_MASKED_SKIN_NAME",
            //    Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
            //    maskedrendererInfos,
            //    mainRenderer,
            //    model);

            //maskedSkin.meshReplacements = new SkinDef.MeshReplacement[]
            //{
            //    //place your mesh replacements here
            //    //unnecessary if you don't have multiple skins
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruMaskMesh"),
            //        renderer = maskedrendererInfos[0].renderer
            //    },
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruWingMesh"),
            //        renderer = maskedrendererInfos[1].renderer
            //    },
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruSwordMesh"),
            //        renderer = maskedrendererInfos[2].renderer
            //    },
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("RimuruHumanMesh"),
            //        renderer = maskedrendererInfos[3].renderer
            //    }
            //};

            //skins.Add(maskedSkin);
            //#endregion
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
            newRendererInfos[1].defaultMaterial = materials[1];
            newRendererInfos[2].defaultMaterial = materials[2];

            return newRendererInfos;
        }
    }
}
