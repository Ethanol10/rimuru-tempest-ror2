using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using HG;
using static UnityEngine.ParticleSystem.PlaybackState;
using System.Collections.Generic;
using UnityEngine.Networking;
using EntityStates.RoboBallBoss.Weapon;
using BullseyeSearch = RoR2.BullseyeSearch;
using TeamMask = RoR2.TeamMask;
using HurtBox = RoR2.HurtBox;
using System.Linq;
using RoR2.Projectile;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Solus Control Unit
     Effect: Reverse Gravity manipulation- enemies get knocked up every X seconds
     */

    public class SolusControlUnitBuffController : RimuruBaseBuffController
    {
        private float timer;
        private int knockupCount = 1;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.bleedMeleeBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Reverse Gravity Manipulation Skill</style> aquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > StaticValues.reverseGravManipInterval)
            {
                timer = 0f;

                RoR2.EffectManager.SimpleMuzzleFlash(FireDelayKnockup.muzzleEffectPrefab, gameObject, "Chest", false);

                if (NetworkServer.active)
                {
                    BullseyeSearch bullseyeSearch = new BullseyeSearch();
                    bullseyeSearch.teamMaskFilter = TeamMask.allButNeutral;
                    if (body.teamComponent)
                    {
                        bullseyeSearch.teamMaskFilter.RemoveTeam(body.teamComponent.teamIndex);
                    }
                    bullseyeSearch.maxDistanceFilter = StaticValues.reverseGravManipRange;
                    bullseyeSearch.maxAngleFilter = 360f;
                    bullseyeSearch.searchOrigin = body.corePosition;
                    bullseyeSearch.searchDirection = Vector3.up;
                    bullseyeSearch.filterByLoS = false;
                    bullseyeSearch.sortMode = BullseyeSearch.SortMode.Angle;
                    bullseyeSearch.RefreshCandidates();
                    List<HurtBox> list = bullseyeSearch.GetResults().ToList<HurtBox>();
                    int num = 0;
                    for (int i = 0; i < this.knockupCount; i++)
                    {
                        if (num >= list.Count)
                        {
                            num = 0;
                        }
                        HurtBox hurtBox = list[num];
                        if (hurtBox)
                        {
                            Vector2 vector = UnityEngine.Random.insideUnitCircle;
                            Vector3 vector2 = hurtBox.transform.position + new Vector3(vector.x, 0f, vector.y);
                            RaycastHit raycastHit;
                            if (Physics.Raycast(new Ray(vector2 + Vector3.up * 1f, Vector3.down), out raycastHit, 200f, RoR2.LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
                            {
                                vector2 = raycastHit.point;
                            }
                            //ProjectileManager.instance.FireProjectile(FireDelayKnockup.projectilePrefab, vector2, Quaternion.identity, base.gameObject, this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                            ProjectileManager.instance.FireProjectile(
                                FireDelayKnockup.projectilePrefab, //prefab
                                vector2, //position
                                Quaternion.identity, //rotation
                            base.gameObject, //owner
                                body.damage* StaticValues.reverseGravManipDamageCoefficient, //damage
                                1f, //force
                                RoR2.Util.CheckRoll(body.crit, master), //crit
                                DamageColorIndex.Default, //damage color
                                null, //target
                                -1f); //speed }}   

                        }
                        num++;
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
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}

