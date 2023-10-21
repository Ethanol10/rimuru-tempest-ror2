using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using IL.RoR2;
using System.Collections.Generic;
using HurtBox = RoR2.HurtBox;
using System.Linq;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Clay Dunestrider 
     Effect: Tar manipulation- enemies get slowed/attackspeed slow nearby
     */

    public class ClayDunestriderBuffController : RimuruBaseBuffController
    {
        private GameObject tarManipIndicator;
        private float timer;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.tarManipBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            RoR2.Chat.AddMessage("<style=cIsUtility>Tar Manipulation Skill</style> aquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.fixedDeltaTime;
            if (timer > 1f)
            {
                timer = 0f;
                ApplyTar();
            }
        }

        public void Update()
        {
            if (!this.tarManipIndicator)
            {
                CreateTarManipIndicator();
            }
            if (tarManipIndicator)
            {
                this.tarManipIndicator.transform.parent = body.transform;
                this.tarManipIndicator.transform.localScale = Vector3.one * StaticValues.tarManipRadius;
                this.tarManipIndicator.transform.localPosition = Vector3.zero;

            }

        }

        public void CreateTarManipIndicator()
        {
            this.tarManipIndicator = UnityEngine.Object.Instantiate<GameObject>(Assets.tarManipIndicatorPrefab);
            this.tarManipIndicator.SetActive(true);

            this.tarManipIndicator.transform.parent = body.transform;
            this.tarManipIndicator.transform.localScale = Vector3.one * StaticValues.tarManipRadius;
            this.tarManipIndicator.transform.localPosition = Vector3.zero;


        }


        public void ApplyTar()
        {
            RoR2.BullseyeSearch search = new RoR2.BullseyeSearch
            {

                teamMaskFilter = RoR2.TeamMask.GetEnemyTeams(TeamIndex.Player),
                filterByLoS = false,
                searchOrigin = body.corePosition,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = RoR2.BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = StaticValues.tarManipRadius,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(body.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        if(singularTarget.healthComponent.body.HasBuff(Buffs.tarManipDebuff))
                        {
                            singularTarget.healthComponent.body.ApplyBuff(Buffs.tarManipDebuff.buffIndex, 1, 3);
                        }
                    }
                }
            }
        }

        public override void OnDestroy()
        {
            base.FixedUpdate();
            if (tarManipIndicator)
            {
                tarManipIndicator.SetActive(false);
                UnityEngine.GameObject.Destroy(tarManipIndicator);
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

