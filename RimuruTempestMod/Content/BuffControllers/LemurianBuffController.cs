using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using System.Collections.Generic;
using System.Linq;
using HurtBox = RoR2.HurtBox;
using DotController = RoR2.DotController;
using EntityStates;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Lemurian
     Effect: Flame Body - Flame aura around body
     */

    public class LemurianBuffController : RimuruBaseBuffController
    {
        private float timer;
        private GameObject flameBodyIndicator;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = true;
            buffdef = Buffs.flameBodyBuff;
        }

        public void Start()
        {
            RoR2.Chat.AddMessage("<style=cIsUtility>Flame Body Skill</style> aquisition successful.");
        }


        public void Update()
        {
            if (!this.flameBodyIndicator)
            {
                CreateFlameBodyIndicator();
            }
            if (flameBodyIndicator)
            {
                this.flameBodyIndicator.transform.parent = body.transform;
                this.flameBodyIndicator.transform.localScale = Vector3.one * StaticValues.flameBodyRadius;
                this.flameBodyIndicator.transform.localPosition = body.corePosition;

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (timer < 0f)
            {
                timer += Time.fixedDeltaTime;
            }
            else if (timer > StaticValues.flameBodyThreshold)
            {
                timer = 0f;
                Burn();
            }
        }


        public override void OnDestroy()
        {
            base.OnDestroy();

            if (flameBodyIndicator)
            {
                flameBodyIndicator.SetActive(false);
                EntityState.Destroy(flameBodyIndicator.gameObject);
            }
        }


        public void CreateFlameBodyIndicator()
        {
            this.flameBodyIndicator = UnityEngine.Object.Instantiate<GameObject>(Assets.flameBodyAuraIndicatorPrefab);
            this.flameBodyIndicator.SetActive(true);

            this.flameBodyIndicator.transform.localScale = Vector3.one * StaticValues.flameBodyRadius;
            this.flameBodyIndicator.transform.localPosition = body.corePosition;


        }

        public void Burn()
        {
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
            search.FilterOutGameObject(body.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget.healthComponent)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        InflictDotInfo info = new InflictDotInfo();
                        info.attackerObject = body.gameObject;
                        info.victimObject = singularTarget.healthComponent.body.gameObject;
                        info.duration = StaticValues.flameBodyDuration;
                        info.dotIndex = DotController.DotIndex.Burn;
                        info.totalDamage = body.damage * StaticValues.flameBodyDamageCoefficient;
                        info.damageMultiplier = 1f;

                        RoR2.StrengthenBurnUtils.CheckDotForUpgrade(body.inventory, ref info);
                        DotController.InflictDot(ref info);
                    }
                }
            }


        }

    }
}

