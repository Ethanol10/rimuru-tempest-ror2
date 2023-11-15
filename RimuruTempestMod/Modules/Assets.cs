using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;
using UnityEngine.AddressableAssets;
using RoR2.Projectile;

namespace RimuruMod.Modules
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;
        public static GameObject blacklightning;
        public static GameObject blacklightningimpactEffect;
        public static GameObject devourEffect;
        public static GameObject devourskillgetEffect;
        public static GameObject analyzeEffect;
        public static GameObject waterbladeimpactEffect;
        public static GameObject wetEffect;
        internal static List<GameObject> networkObjDefs = new List<GameObject>();

        //internal static GameObject bombExplosionEffect;

        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;

        // cache these and use to create our own materials
        internal static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;
        private static string[] assetNames = new string[0];

        // CHANGE THIS
        private const string assetbundleName = "RimuruAssetBundle";
        //change this to your project's name if/when you've renamed it
        private const string csProjName = "RimuruTempestMod";

        //Materials
        public static Material SpatialMovementBuffMaterial;


        //buffs
        public static Sprite strongerBurnIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/StrengthenBurn/bdStrongerBurn.asset").WaitForCompletion().iconSprite;
        public static Sprite lunarRootIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/LunarSkillReplacements/bdLunarSecondaryRoot.asset").WaitForCompletion().iconSprite;
        //public static Sprite strongerBurnIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/StrengthenBurn/bdStrongerBurn.asset").WaitForCompletion().iconSprite;
        //public static Sprite mercExposeIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Merc/bdMercExpose.asset").WaitForCompletion().iconSprite;
        //public static Sprite deathMarkDebuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/DeathMark/bdDeathMark.asset").WaitForCompletion().iconSprite;
        //public static Sprite singularityBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/ElementalRingVoid/bdElementalRingVoidReady.asset").WaitForCompletion().iconSprite;
        //public static Sprite cloakBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdCloak.asset").WaitForCompletion().iconSprite;
        //public static Sprite voidFogDebuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdVoidFogMild.asset").WaitForCompletion().iconSprite;
        public static Sprite medkitBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Medkit/bdMedkitHeal.asset").WaitForCompletion().iconSprite;
        //public static Sprite spikyDebuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Treebot/bdEntangle.asset").WaitForCompletion().iconSprite;
        public static Sprite ruinDebuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/LunarSkillReplacements/bdLunarDetonationCharge.asset").WaitForCompletion().iconSprite;
        //public static Sprite warcryBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/WarCryOnMultiKill/bdWarCryBuff.asset").WaitForCompletion().iconSprite;
        public static Sprite shieldBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdArmorBoost.asset").WaitForCompletion().iconSprite;
        //public static Sprite tarBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdClayGoo.asset").WaitForCompletion().iconSprite;
        public static Sprite crippleBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdCripple.asset").WaitForCompletion().iconSprite;
        public static Sprite speedBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdCloakSpeed.asset").WaitForCompletion().iconSprite;
        public static Sprite boostBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/RandomDamageZone/bdPowerBuff.asset").WaitForCompletion().iconSprite;
        public static Sprite bearVoidReadyBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/BearVoid/bdBearVoidReady.asset").WaitForCompletion().iconSprite;
        public static Sprite alphashieldoffBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/BearVoid/bdBearVoidCooldown.asset").WaitForCompletion().iconSprite;
        //public static Sprite decayBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdVoidFogStrong.asset").WaitForCompletion().iconSprite;
        public static Sprite shurikenBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/PrimarySkillShuriken/bdPrimarySkillShurikenBuff.asset").WaitForCompletion().iconSprite;
        public static Sprite sprintBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/SprintOutOfCombat/bdWhipBoost.asset").WaitForCompletion().iconSprite;
        public static Sprite spikeBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Grandparent/bdOverheat.asset").WaitForCompletion().iconSprite;
        public static Sprite mortarBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/GainArmor/bdElephantArmorBoost.asset").WaitForCompletion().iconSprite;
        public static Sprite healBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Croco/bdCrocoRegen.asset").WaitForCompletion().iconSprite;
        public static Sprite attackspeedBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/EnergizedOnEquipmentUse/bdEnergized.asset").WaitForCompletion().iconSprite;
        public static Sprite noCooldownBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/KillEliteFrenzy/bdNoCooldowns.asset").WaitForCompletion().iconSprite;
        public static Sprite bleedBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdBleeding.asset").WaitForCompletion().iconSprite;
        //public static Sprite skinBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/OutOfCombatArmor/bdOutOfCombatArmorBuff.asset").WaitForCompletion().iconSprite;
        //public static Sprite orbreadyBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/ElementalRings/bdElementalRingsReady.asset").WaitForCompletion().iconSprite;
        //public static Sprite orbdisableBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/ElementalRings/bdElementalRingsCooldown.asset").WaitForCompletion().iconSprite;
        //public static Sprite blazingBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion().iconSprite;
        public static Sprite lightningBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/ShockNearby/bdTeslaField.asset").WaitForCompletion().iconSprite;
        public static Sprite resonanceBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/LaserTurbine/bdLaserTurbineKillCharge.asset").WaitForCompletion().iconSprite;
        public static Sprite critBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/CritOnUse/bdFullCrit.asset").WaitForCompletion().iconSprite;
        public static Sprite claygooBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdClayGoo.asset").WaitForCompletion().iconSprite;
        //public static Sprite predatorBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/AttackSpeedOnCrit/bdAttackSpeedOnCrit.asset").WaitForCompletion().iconSprite;
        public static Sprite fireBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion().iconSprite;
        public static Sprite jumpBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/MoveSpeedOnKill/bdKillMoveSpeed.asset").WaitForCompletion().iconSprite;

        //effects
        public static GameObject elderlemurianexplosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/OmniExplosionVFXLemurianBruiserFireballImpact.prefab").WaitForCompletion();
        public static GameObject voidjailerEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerDeathBombExplosion.prefab").WaitForCompletion();
        public static GameObject railgunnerSnipeLightTracerEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/TracerRailgunLight.prefab").WaitForCompletion();
        public static GameObject GupSpikeEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupExplosion.prefab").WaitForCompletion();
        public static GameObject loaderOmniImpactLightningEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/OmniImpactVFXLoaderLightning.prefab").WaitForCompletion();

        //objects        
        public static GameObject spherePrefab;
        public static GameObject flameBodyAuraIndicatorPrefab;
        public static GameObject tarManipIndicatorPrefab;


        internal static void Initialize()
        {
            if (assetbundleName == "myassetbundle")
            {
                Log.Error("AssetBundle name hasn't been changed. not loading any assets to avoid conflicts");
                return;
            }

            LoadAssetBundle();
            LoadSoundbank();
            PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            try
            {
                if (mainAssetBundle == null)
                {
                    using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.{assetbundleName}"))
                    {
                        mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to load assetbundle. Make sure your assetbundle name is setup correctly\n" + e);
                return;
            }

            assetNames = mainAssetBundle.GetAllAssetNames();
        }

        internal static void LoadSoundbank()
        {                                                                
            //soundbank currently broke, but this is how you should load yours
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.RimuruBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        internal static void PopulateAssets()
        {
            if (!mainAssetBundle)
            {
                Log.Error("There is no AssetBundle to load assets from.");
                return;
            }

            // feel free to delete everything in here and load in your own assets instead
            // it should work fine even if left as is- even if the assets aren't in the bundle


            //SpatialMovementBuff effect
            SpatialMovementBuffMaterial = mainAssetBundle.LoadAsset<Material>("SpatialMovementMat");

            //blacklightning beam effect
            blacklightning = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("BlackLightning");
            blacklightning.AddComponent<NetworkIdentity>();
            networkObjDefs.Add(blacklightning);
            PrefabAPI.RegisterNetworkPrefab(blacklightning);
            //blacklightningimpact effect
            blacklightningimpactEffect = LoadEffect("BlackLightningImpact");
            //devour effect
            devourEffect = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("Devour");
            devourskillgetEffect = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("SkillGet");
            //analyze effect
            analyzeEffect = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("Analyze");
            //waterblade impact effect
            waterbladeimpactEffect = LoadEffect("WaterBladeImpact");
            //wet effect
            wetEffect = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("WetEffect");

            //sword
            swordHitSoundEvent = CreateNetworkSoundEventDef("RimuruHit");

            //swordSwingEffect = Assets.LoadEffect("RimuruSwordSwing", true);
            swordSwingEffect = Assets.LoadEffect("RimuruSwordSwing", true);
            swordHitImpactEffect = Assets.LoadEffect("ImpactRimuruSlash");
            networkObjDefs.Add(swordSwingEffect);
            networkObjDefs.Add(swordHitImpactEffect);
            PrefabAPI.RegisterNetworkPrefab(swordSwingEffect);
            PrefabAPI.RegisterNetworkPrefab(swordHitImpactEffect);

            //warbanner material setup
            Material warbannerMat = Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/WardOnLevel/matWarbannerSphereIndicator.mat").WaitForCompletion();
            Material[] warbannerArray = new Material[1];
            warbannerArray[0] = warbannerMat;

            //objects

            spherePrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("SphereIndicator");
            spherePrefab.AddComponent<NetworkIdentity>();

            flameBodyAuraIndicatorPrefab = PrefabAPI.InstantiateClone(spherePrefab, "flameBodyAuraIndicatorPrefab");

            MeshRenderer flameBodyAuraIndicatorMeshRender = flameBodyAuraIndicatorPrefab.gameObject.GetComponent<MeshRenderer>();
            if (!flameBodyAuraIndicatorMeshRender)
            {
                Debug.Log("Failed to find Mesh renderer!");
            }
            flameBodyAuraIndicatorMeshRender.materials = warbannerArray;
            flameBodyAuraIndicatorMeshRender.material.SetColor("_TintColor", new Color(184f / 255f, 24f / 255f, 24f / 255f)); //new Color(153/255f, 21/255f, 63/255f)
            networkObjDefs.Add(flameBodyAuraIndicatorPrefab);
            PrefabAPI.RegisterNetworkPrefab(flameBodyAuraIndicatorPrefab);


            tarManipIndicatorPrefab = PrefabAPI.InstantiateClone(spherePrefab, "tarManipIndicatorPrefab");

            MeshRenderer tarManipIndicatorMeshRender = tarManipIndicatorPrefab.gameObject.GetComponent<MeshRenderer>();
            if (!tarManipIndicatorMeshRender)
            {
                Debug.Log("Failed to find Mesh renderer!");
            }
            tarManipIndicatorMeshRender.materials = warbannerArray;
            tarManipIndicatorMeshRender.material.SetColor("_TintColor", Color.magenta); //new Color(153/255f, 21/255f, 63/255f)
            networkObjDefs.Add(tarManipIndicatorPrefab);
            PrefabAPI.RegisterNetworkPrefab(tarManipIndicatorPrefab);

        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (Renderer i in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                i?.material?.SetHopooMaterial();
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }


        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = mainAssetBundle.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Log.Error("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        internal static Texture LoadCharacterIconGeneric(string characterName)
        {
            return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            bool assetExists = false;
            for (int i = 0; i < assetNames.Length; i++)
            {
                if (assetNames[i].Contains(resourceName.ToLowerInvariant()))
                {
                    assetExists = true;
                    i = assetNames.Length;
                }
            }

            if (!assetExists)
            {
                Log.Error("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            Modules.Content.AddEffectDef(newEffectDef);
        }
    }
}