using UnityEngine;
using RoR2;
using RimuruMod.Modules;
using System.Collections.Generic;
using System.Linq;
using RoR2.Projectile;
using EntityStates.ClayGrenadier;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Clay Apothecary
     Effect: Tarring- Fire Tar at the closest enemies every second
     */

    public class ClayApothecaryBuffController : RimuruBaseBuffController
    {
        private float timer;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.tarringBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Tarring Skill</style> acquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.fixedDeltaTime;
            if(timer > 1f)
            {
                timer = 0f;

                RoR2.EffectManager.SpawnEffect(FaceSlam.blastImpactEffect, new RoR2.EffectData
                {
                    origin = body.footPosition,
                    scale = 1f
                }, true);

                RoR2.BullseyeSearch search = new RoR2.BullseyeSearch
                {

                    teamMaskFilter = RoR2.TeamMask.GetEnemyTeams(body.teamComponent.teamIndex),
                    filterByLoS = false,
                    searchOrigin = body.corePosition,
                    searchDirection = UnityEngine.Random.onUnitSphere,
                    sortMode = RoR2.BullseyeSearch.SortMode.Distance,
                    maxDistanceFilter = StaticValues.flameBodyRadius,
                    maxAngleFilter = 360f
                };
                search.RefreshCandidates();
                search.FilterOutGameObject(base.gameObject);


                List<RoR2.HurtBox> target = search.GetResults().ToList<RoR2.HurtBox>();
                foreach (RoR2.HurtBox singularTarget in target)
                {
                    if (singularTarget)
                    {
                        if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                        {

                            ProjectileManager.instance.FireProjectile(
                                FaceSlam.projectilePrefab, //prefab
                                singularTarget.healthComponent.body.footPosition, //position
                                Quaternion.identity, //rotation
                                base.gameObject, //owner
                                body.damage* StaticValues.tarringDamageCoefficient, //damage
                                0f, //force
                                RoR2.Util.CheckRoll(body.crit, body.master), //crit
                                DamageColorIndex.Default, //damage color
                                null, //target
                                -1f); //speed

                        }
                    }
                }
            }
        }


        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }


    }
}

