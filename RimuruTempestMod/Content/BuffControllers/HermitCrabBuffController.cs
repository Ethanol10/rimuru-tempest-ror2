using System;
using UnityEngine;
using RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruMod.Modules.Survivors;
using RoR2.Orbs;
using EntityStates;
using EntityStates.Huntress;
using System.Collections.Generic;
using System.Linq;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Hermit Crab
     Effect: Mortaring- Standing still shoots projectiles at nearby enemies
     */

    public class HermitCrabBuffController : RimuruBaseBuffController
    {
        private GameObject mortarIndicatorInstance;
        private float mortarTimer;

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.hermitMortarBuff;
        }

        public void Start()
        {
            RoR2.Chat.AddMessage("<style=cIsUtility>Mortaring Skill</style> acquisition successful.");
        }

        public void Hook()
        {
            
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if(mortarIndicatorInstance)
            {
                mortarIndicatorInstance.SetActive(false);
                EntityState.Destroy(mortarIndicatorInstance.gameObject);
            }
        }

        public void Update()
        {

            if (mortarIndicatorInstance)
            {
                this.mortarIndicatorInstance.transform.parent = body.transform;
                this.mortarIndicatorInstance.transform.localScale = Vector3.one * StaticValues.hermitMortarRadius;

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (body.hasEffectiveAuthority)
            {

                //Standing still/not moving buffs
                if (body.GetNotMoving())
                {
                    //hermitcrab mortarbuff
                    //Debug.Log(mortarIndicatorInstance + "exists mortar indicator");
                    if (!this.mortarIndicatorInstance)
                    {
                        CreateMortarIndicator();
                    }

                    mortarTimer += Time.fixedDeltaTime;
                    if (mortarTimer >= StaticValues.mortarbaseDuration / (body.attackSpeed))
                    {
                        mortarTimer = 0f;
                        FireMortar();

                    }

                    
                }
                else if (!body.GetNotMoving())
                {
                    if (this.mortarIndicatorInstance)
                    {
                        mortarIndicatorInstance.SetActive(false);
                        EntityState.Destroy(this.mortarIndicatorInstance.gameObject);
                    }

                }
            }
        }

        //hermit crab mortar
        public void CreateMortarIndicator()
        {
            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.mortarIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.mortarIndicatorInstance.SetActive(true);

                this.mortarIndicatorInstance.transform.parent = body.transform;
                this.mortarIndicatorInstance.transform.localScale = Vector3.one * StaticValues.hermitMortarRadius;
                this.mortarIndicatorInstance.transform.localPosition = Vector3.zero;

            }

            
        }


        public void FireMortar()
        {
            MortarOrb mortarOrb = new MortarOrb
            {
                attacker = body.gameObject,
                damageColorIndex = DamageColorIndex.Default,
                damageValue = body.damage * StaticValues.mortarDamageCoefficient,
                origin = body.corePosition,
                procChainMask = new RoR2.ProcChainMask(),
                procCoefficient = 1f,
                isCrit = RoR2.Util.CheckRoll(body.crit, body.master),
                teamIndex = body.teamComponent.teamIndex,
            };
            if (mortarOrb.target = mortarOrb.PickNextTarget(mortarOrb.origin, StaticValues.hermitMortarRadius))
            {
                OrbManager.instance.AddOrb(mortarOrb);
            }

            RoR2.EffectManager.SpawnEffect(EntityStates.HermitCrab.FireMortar.mortarMuzzleflashEffect, new RoR2.EffectData
            {
                origin = body.corePosition,
                scale = 1f,
                rotation = Quaternion.identity,

            }, true);




        }


    }

    //mortar orb
    public class MortarOrb : Orb
    {
        public override void Begin()
        {
            base.duration = 0.5f;
            EffectData effectData = new EffectData
            {
                origin = this.origin,
                genericFloat = base.duration
            };
            effectData.SetHurtBoxReference(this.target);
            GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/SquidOrbEffect");
            EffectManager.SpawnEffect(effectPrefab, effectData, true);
        }
        public HurtBox PickNextTarget(Vector3 position, float range)
        {
            BullseyeSearch bullseyeSearch = new BullseyeSearch();
            bullseyeSearch.searchOrigin = position;
            bullseyeSearch.searchDirection = Vector3.zero;
            bullseyeSearch.teamMaskFilter = TeamMask.all;
            bullseyeSearch.teamMaskFilter.RemoveTeam(this.teamIndex);
            bullseyeSearch.filterByLoS = false;
            bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
            bullseyeSearch.maxDistanceFilter = range;
            bullseyeSearch.RefreshCandidates();
            List<HurtBox> list = bullseyeSearch.GetResults().ToList<HurtBox>();
            if (list.Count <= 0)
            {
                return null;
            }
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public override void OnArrival()
        {
            if (this.target)
            {
                HealthComponent healthComponent = this.target.healthComponent;
                if (healthComponent)
                {
                    DamageInfo damageInfo = new DamageInfo
                    {
                        damage = this.damageValue,
                        attacker = this.attacker,
                        inflictor = null,
                        force = Vector3.zero,
                        crit = this.isCrit,
                        procChainMask = this.procChainMask,
                        procCoefficient = this.procCoefficient,
                        position = this.target.transform.position,
                        damageColorIndex = this.damageColorIndex
                    };
                    healthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
                }
            }
        }

        public float damageValue;
        public GameObject attacker;
        public TeamIndex teamIndex;
        public bool isCrit;
        public ProcChainMask procChainMask;
        public float procCoefficient = 1f;
        public DamageColorIndex damageColorIndex;
    }
}

