using EntityStates;
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EntityStates.LunarExploderMonster;
using RoR2.Projectile;
using EntityStates.MiniMushroom;
using UnityEngine.Networking;
using R2API.Networking;

namespace RimuruMod.Modules.Survivors
{
    public class RimuruController : MonoBehaviour
    {
        string prefix = RimuruSlime.RIMURU_PREFIX;
        public GameObject devoureffectObj;

        public float strengthMultiplier;
        public float rangedMultiplier;

        public float AFOTimer;
        public float overloadingtimer;
        public float magmawormtimer;
        public float vagranttimer;
        public float alphaconstructshieldtimer;
        public float lunarTimer;
        public float larvaTimer;
        public float attackSpeedGain;
        public float mortarTimer;

        public Transform mortarIndicatorInstance;
        public Transform voidmortarIndicatorInstance;

        public float maxTrackingDistance = 60f;
        public float maxTrackingAngle = 30f;
        public float trackerUpdateFrequency = 10f;
        private Indicator indicator;
        private HurtBox trackingTarget;
        public HurtBox Target;

        private CharacterBody characterBody;
        private InputBankTest inputBank;
        private float trackerUpdateStopwatch;
        private ChildLocator child;
        private readonly BullseyeSearch search = new BullseyeSearch();
        private CharacterMaster characterMaster;

        public RimuruMasterController Rimurumastercon;
        public RimuruController Rimurucon;

        public uint loopID;

        public void Awake()
        {
            child = GetComponentInChildren<ChildLocator>();

            if (devoureffectObj)
            {
                Destroy(devoureffectObj);
            }

            indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            characterBody = gameObject.GetComponent<CharacterBody>();
            inputBank = gameObject.GetComponent<InputBankTest>();


        }


        public void Start()
        {

            characterMaster = characterBody.master;
            if (!characterMaster.gameObject.GetComponent<RimuruMasterController>())
            {
                Rimurumastercon = characterMaster.gameObject.AddComponent<RimuruMasterController>();
            }

            characterBody.skillLocator.special.RemoveAllStocks();


        }

        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }

        private void OnEnable()
        {
            this.indicator.active = true;
        }

        private void OnDisable()
        {
            this.indicator.active = false;
        }


        public void FixedUpdate()
        {

            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
                this.SearchForTarget(aimRay);
                HurtBox hurtBox = this.trackingTarget;
                this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);

                
            }



            //devour effect
            if (characterBody.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
            {
                bool buttonHeld = false;
                if (characterBody.inputBank.skill1.down )
                {
                    buttonHeld = true;
                    if (!devoureffectObj && buttonHeld)
                    {
                        this.loopID = AkSoundEngine.PostEvent(1183893824, base.gameObject);
                        devoureffectObj = Instantiate(Modules.Assets.devourEffect, child.FindChild("Spine").transform.position, Quaternion.LookRotation(characterBody.characterDirection.forward));
                    }

                }
                else
                {
                    AkSoundEngine.StopPlayingID(this.loopID);
                    if (devoureffectObj)
                    {
                        Destroy(devoureffectObj);
                    }

                }

            }

        }


        public void Update()
        {
            if (devoureffectObj)
            {
                devoureffectObj.transform.position = child.FindChild("Spine").transform.position;
                devoureffectObj.transform.rotation = Quaternion.LookRotation(characterBody.characterDirection.forward);
            }
        }

        private void SearchForTarget(Ray aimRay)
        {
            this.search.teamMaskFilter = TeamMask.all;
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.maxTrackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
        }

        public void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(this.loopID);
            Destroy(devoureffectObj);
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
            bullseyeSearch.teamMaskFilter = TeamMask.allButNeutral;
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


