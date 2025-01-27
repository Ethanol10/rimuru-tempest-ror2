using R2API;
using R2API.Networking;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.Modules
{
    internal static class Projectiles
    {
        //internal static GameObject bombPrefab;
        internal static GameObject waterbladeProjectile;
        internal static GameObject icicleLanceProjectile;

        internal static void RegisterProjectiles()
        {
            CreateIcicle();
            CreateWaterBlade();
            AddProjectile(waterbladeProjectile);
            AddProjectile(icicleLanceProjectile);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateIcicle()
        {
            icicleLanceProjectile = CloneProjectilePrefab("MageIceBombProjectile", "RimuruIcicleProjectile");

            //ProjectileImpactExplosion iceImpactExplosion = icicleLanceProjectile.GetComponent<ProjectileImpactExplosion>();
            //InitializeImpactExplosion(iceImpactExplosion);

            //iceImpactExplosion.blastDamageCoefficient = 2f;
            //iceImpactExplosion.blastRadius = 4f;
            //iceImpactExplosion.destroyOnEnemy = true;
            //iceImpactExplosion.lifetime = 12f;
            //iceImpactExplosion.timerAfterImpact = true;
            //iceImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController icicleController = icicleLanceProjectile.GetComponent<ProjectileController>();

            icicleController.ghostPrefab = Modules.AssetsRimuru.mageIceBombGhost;

        }


        private static void CreateWaterBlade() 
        {
            waterbladeProjectile = Modules.AssetsRimuru.mainAssetBundle.LoadAsset<GameObject>("WaterBlade");
            // Ensure that the child is set in the right position in Unity!!!!
            Modules.Prefabs.SetupHitbox(waterbladeProjectile, waterbladeProjectile.transform.GetChild(0), "waterblade");
            waterbladeProjectile.AddComponent<NetworkIdentity>();
            
            ProjectileController waterbladeProjectileCon = waterbladeProjectile.AddComponent<ProjectileController>();
           
            ProjectileDamage waterbladeProjectileDamage = waterbladeProjectile.AddComponent<ProjectileDamage>();
            InitializeWaterBladeDamage(waterbladeProjectileDamage);
            
            ProjectileSimple waterbladeProjectileTrajectory = waterbladeProjectile.AddComponent<ProjectileSimple>();
            InitializeWaterBladeTrajectory(waterbladeProjectileTrajectory);

            ProjectileOverlapAttack waterbladeoverlapAttack = waterbladeProjectile.AddComponent<ProjectileOverlapAttack>();
            InitializeWaterBladeOverlapAttack(waterbladeoverlapAttack);
            waterbladeProjectile.AddComponent<WaterbladeOnHit>();

            //Waterblade Damage
            waterbladeProjectileCon.procCoefficient = 1.0f;
            waterbladeProjectileCon.canImpactOnTrigger = true;

            PrefabAPI.RegisterNetworkPrefab(waterbladeProjectile);
        }

        internal static void InitializeWaterBladeOverlapAttack(ProjectileOverlapAttack overlap) 
        {
            overlap.overlapProcCoefficient = 1.0f;
            overlap.damageCoefficient = 1.0f;
            overlap.impactEffect = Modules.AssetsRimuru.waterbladeimpactEffect;
        }

        internal static void InitializeWaterBladeTrajectory(ProjectileSimple simple) 
        {
            simple.lifetime = Modules.StaticValues.waterbladeProjectileLifetime;
            simple.desiredForwardSpeed = Modules.StaticValues.waterbladeProjectileSpeed;

        }

        internal static void InitializeWaterBladeDamage(ProjectileDamage damageComponent) 
        {
            damageComponent.damage = Modules.Config.waterbladeDamageCoefficient.Value;
            damageComponent.crit = false;
            damageComponent.force = Modules.StaticValues.waterbladeForce;
            damageComponent.damageType = new RoR2.DamageTypeCombo(DamageType.Generic, DamageTypeExtended.Generic, DamageSource.Secondary);
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

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = new RoR2.DamageTypeCombo(DamageType.Freeze2s, DamageTypeExtended.Generic, DamageSource.NoneSpecified);
            
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.AssetsRimuru.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.AssetsRimuru.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }

        internal class WaterbladeOnHit : MonoBehaviour, IProjectileImpactBehavior
        {
            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                if (impactInfo.collider) 
                {
                    GameObject collidedObject = impactInfo.collider.gameObject;
                    CharacterBody body = collidedObject.GetComponent<CharacterBody>();
                    if (body) 
                    {
                        if (body.teamComponent)
                        {
                            if (body.teamComponent.teamIndex == TeamIndex.Neutral || body.teamComponent.teamIndex == TeamIndex.Monster
                                || body.teamComponent.teamIndex == TeamIndex.Lunar || body.teamComponent.teamIndex == TeamIndex.Void)
                            {
                                body.ApplyBuff(Buffs.WetDebuff.buffIndex, 1, Config.waterbladeWetDebuffDuration.Value);
                                //do something to enemy
                                //if (NetworkServer.active)
                                //{
                                //    body.AddTimedBuff(Modules.Buffs.WetDebuff, Modules.Config.waterbladeWetDebuffDuration.Value);
                                //}
                                //MIGHT need to do some more networking if the projectile doesn't register debuffs.
                                //This is as simple as getting the masterobjectID from the body and applying the debuff in a network call. No bigs.
                            }
                        }
                    }
                }
            }
        }
    }
}
