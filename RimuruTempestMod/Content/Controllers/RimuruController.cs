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
        public GameObject devoureffectExtendedObj;

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
            else
            {
                Rimurumastercon = characterMaster.gameObject.GetComponent<RimuruMasterController>();
            }

            Rimurumastercon.isBodyInitialized = false;

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

                    if (characterBody.HasBuff(Buffs.devourBuff))
                    {
                        if (!devoureffectExtendedObj && buttonHeld)
                        {
                            this.loopID = AkSoundEngine.PostEvent("RimuruDevour", base.gameObject);
                            devoureffectExtendedObj = Instantiate(Modules.AssetsRimuru.devourExtendedEffect, child.FindChild("Spine").transform.position, Quaternion.LookRotation(characterBody.characterDirection.forward));
                        }
                    }
                    else
                    {
                        if (!devoureffectObj && buttonHeld)
                        {
                            this.loopID = AkSoundEngine.PostEvent("RimuruDevour", base.gameObject);
                            devoureffectObj = Instantiate(Modules.AssetsRimuru.devourEffect, child.FindChild("Spine").transform.position, Quaternion.LookRotation(characterBody.characterDirection.forward));
                        }
                    }


                }
                else
                {
                    AkSoundEngine.StopPlayingID(this.loopID);
                    if (devoureffectObj)
                    {
                        Destroy(devoureffectObj);
                    }
                    if (devoureffectExtendedObj)
                    {
                        Destroy(devoureffectExtendedObj);
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
            if (devoureffectExtendedObj)
            {
                devoureffectExtendedObj.transform.position = child.FindChild("Spine").transform.position;
                devoureffectExtendedObj.transform.rotation = Quaternion.LookRotation(characterBody.characterDirection.forward);
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







}


