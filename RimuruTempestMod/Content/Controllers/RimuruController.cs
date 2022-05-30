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

namespace RimuruMod.Modules.Survivors
{
    public class RimuruController : MonoBehaviour
    {
        string prefix = RimuruSlime.RIMURU_PREFIX;

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
        private float voidmortarTimer;
        private float voidjailerTimer;
        private float roboballTimer;

        private Ray downRay;
        public Transform mortarIndicatorInstance;
        public Transform voidmortarIndicatorInstance;

        public float maxTrackingDistance = 60f;
        public float maxTrackingAngle = 60f;
        public float trackerUpdateFrequency = 10f;
        private Indicator indicator;
        private Indicator passiveindicator;
        private Indicator activeindicator;
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

        public bool larvabuffGiven;
        public bool verminjumpbuffGiven;
        private uint minimushrumsoundID;
        public GameObject mushroomWard;
        public GameObject magmawormWard;
        public GameObject overloadingWard;

        public bool alphacontructpassiveDef;
        public bool beetlepassiveDef;
        public bool pestpassiveDef;
        public bool verminpassiveDef;
        public bool guppassiveDef;
        public bool hermitcrabpassiveDef;
        public bool larvapassiveDef;
        public bool lesserwisppassiveDef;
        public bool lunarexploderpassiveDef;
        public bool minimushrumpassiveDef;
        public bool roboballminibpassiveDef;
        public bool voidbarnaclepassiveDef;
        public bool voidjailerpassiveDef;
        public bool impbosspassiveDef;
        public bool stonetitanpassiveDef;
        public bool magmawormpassiveDef;
        public bool overloadingwormpassiveDef;

        public bool alloyvultureflyDef;
        public bool beetleguardslamDef;
        public bool bisonchargeDef;
        public bool bronzongballDef;
        public bool clayapothecarymortarDef;
        public bool claytemplarminigunDef;
        public bool greaterwispballDef;
        public bool impblinkDef;
        public bool jellyfishnovaDef;
        public bool lemurianfireballDef;
        public bool lunargolemshotsDef;
        public bool lunarwispminigunDef;
        public bool parentteleportDef;
        public bool stonegolemlaserDef;
        public bool voidreaverportalDef;
        public bool beetlequeenshotgunDef;
        public bool grandparentsunDef;
        public bool grovetenderhookDef;
        public bool claydunestriderballDef;
        public bool soluscontrolunityknockupDef;
        public bool xiconstructbeamDef;
        public bool voiddevastatorhomingDef;
        public bool scavengerthqwibDef;

        public bool hasExtra1;
        public bool hasExtra2;
        public bool hasExtra3;
        public bool hasExtra4;
        public bool hasQuirk;
        public float quirkTimer;

        public float shiggyDamage;
        public int projectileCount;
        public int decayCount;
        public int captainitemcount;
        private DamageType damageType;
        private DamageType damageType2;
        private float effecttimer1;
        private float effecttimer2;
        private float effecttimer3;

        public void Awake()
        {
            child = GetComponentInChildren<ChildLocator>();

            indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            characterBody = gameObject.GetComponent<CharacterBody>();
            inputBank = gameObject.GetComponent<InputBankTest>();

            larvabuffGiven = false;
            verminjumpbuffGiven = false;
            strengthMultiplier = 1f;
            rangedMultiplier = 1f;

            alphacontructpassiveDef = false;
            beetlepassiveDef = false;
            pestpassiveDef = false;
            verminpassiveDef = false;
            guppassiveDef = false;
            hermitcrabpassiveDef = false;
            larvapassiveDef = false;
            lesserwisppassiveDef = false;
            lunarexploderpassiveDef = false;
            minimushrumpassiveDef = false;
            roboballminibpassiveDef = false;
            voidbarnaclepassiveDef = false;
            voidjailerpassiveDef = false;


            impbosspassiveDef = false;
            stonetitanpassiveDef = false;
            magmawormpassiveDef = false;
            overloadingwormpassiveDef = false;


            alloyvultureflyDef = false;
            beetleguardslamDef = false;
            bisonchargeDef = false;
            bronzongballDef = false;
            clayapothecarymortarDef = false;
            claytemplarminigunDef = false;
            greaterwispballDef = false;
            impblinkDef = false;
            jellyfishnovaDef = false;
            lemurianfireballDef = false;
            lunargolemshotsDef = false;
            lunarwispminigunDef = false;
            parentteleportDef = false;
            stonegolemlaserDef = false;
            voidreaverportalDef = false;

            beetlequeenshotgunDef = false;
            grovetenderhookDef = false;
            grandparentsunDef = false;
            claydunestriderballDef = false;
            soluscontrolunityknockupDef = false;
            xiconstructbeamDef = false;
            voiddevastatorhomingDef = false;
            scavengerthqwibDef = false;

            hasQuirk = false;
            hasExtra1 = false;
            hasExtra2 = false;
            hasExtra3 = false;
            hasExtra4 = false;

        }


        public void Start()
        {

            characterMaster = characterBody.master;
            if (!characterMaster.gameObject.GetComponent<RimuruMasterController>())
            {
                Rimurumastercon = characterMaster.gameObject.AddComponent<RimuruMasterController>();
            }

            characterBody.skillLocator.special.RemoveAllStocks();

            alphacontructpassiveDef = false;
            beetlepassiveDef = false;
            pestpassiveDef = false;
            verminpassiveDef = false;
            guppassiveDef = false;
            hermitcrabpassiveDef = false;
            larvapassiveDef = false;
            lesserwisppassiveDef = false;
            lunarexploderpassiveDef = false;
            minimushrumpassiveDef = false;
            roboballminibpassiveDef = false;
            voidbarnaclepassiveDef = false;
            voidjailerpassiveDef = false;

            impbosspassiveDef = false;
            stonetitanpassiveDef = false;
            magmawormpassiveDef = false;
            overloadingwormpassiveDef = false;


            alloyvultureflyDef = false;
            beetleguardslamDef = false;
            bisonchargeDef = false;
            bronzongballDef = false;
            clayapothecarymortarDef = false;
            claytemplarminigunDef = false;
            greaterwispballDef = false;
            impblinkDef = false;
            jellyfishnovaDef = false;
            lemurianfireballDef = false;
            lunargolemshotsDef = false;
            lunarwispminigunDef = false;
            parentteleportDef = false;
            stonegolemlaserDef = false;
            voidreaverportalDef = false;

            beetlequeenshotgunDef = false;
            grovetenderhookDef = false;
            grandparentsunDef = false;
            claydunestriderballDef = false;
            soluscontrolunityknockupDef = false;
            xiconstructbeamDef = false;
            voiddevastatorhomingDef = false;
            scavengerthqwibDef = false;

        }

        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }

        private void OnEnable()
        {
            this.indicator.active = true;
            this.passiveindicator.active = true;
            this.activeindicator.active = true;
        }

        private void OnDisable()
        {
            this.indicator.active = false;
            this.passiveindicator.active = false;
            this.activeindicator.active = false;
        }

        private void OnDestroy()
        {
            if (mortarIndicatorInstance) EntityState.Destroy(mortarIndicatorInstance.gameObject);
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
                if (hurtBox)
                {
                    this.activeindicator.active = false;
                    this.passiveindicator.active = false;
                    this.indicator.active = true;
                    this.indicator.targetTransform = this.trackingTarget.transform;

                }

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


