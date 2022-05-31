using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject waterbladeProjectile;

        internal static void RegisterProjectiles()
        {
            CreateBomb();

            AddProjectile(bombPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "RimuruBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("RimuruBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("RimuruBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("RimuruBombGhost");
            bombController.startSound = "";
        }

        private static void CreateWaterBlade() 
        {
            waterbladeProjectile = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("WaterBlade");
            Modules.Prefabs.SetupHitbox(waterbladeProjectile, waterbladeProjectile.transform.GetChild(1), "waterblade");
            waterbladeProjectile.AddComponent<NetworkIdentity>();
            ProjectileController waterbladeProjectileCon = waterbladeProjectile.AddComponent<ProjectileController>();
            ProjectileDamage waterbladeProjectileDamage = waterbladeProjectile.AddComponent<ProjectileDamage>();
            ProjectileImpactExplosion waterbladeProjectileImpactExplosion = waterbladeProjectile.AddComponent<ProjectileImpactExplosion>();
            Modules.Projectiles.InitializeImpactExplosion(waterbladeProjectileImpactExplosion);

            //Waterblade Damage
            waterbladeProjectileCon.procCoefficient = 1.0f;

            PrefabAPI.RegisterNetworkPrefab(waterbladeProjectile);


            /*
                Creating projectile:
                - needs ProjectileController
                - needs projectileOverlapAttack
                
             */
        }

        internal static void InitializeWaterBladeDamage(ProjectileDamage damageComponent) 
        {
            damageComponent.damage = Modules.StaticValues.waterbladeDamageCoefficient;
            damageComponent.crit = false;
            damageComponent.force = Modules.StaticValues.waterbladeForce;
            damageComponent.damageType = DamageType.Generic;
        }

        internal static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 0f;
            projectileImpactExplosion.blastProcCoefficient = 0f;
            projectileImpactExplosion.blastRadius = 0f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = true;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 10f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}