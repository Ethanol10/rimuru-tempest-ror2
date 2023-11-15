using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using IL.RoR2;
using RimuruMod.Modules.Survivors;
using RoR2.Orbs;
using EntityStates;
using EntityStates.Huntress;

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
                this.mortarIndicatorInstance.transform.localPosition = body.corePosition;

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
            this.mortarIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.mortarIndicatorInstance.SetActive(true);

            this.mortarIndicatorInstance.transform.parent = body.transform;
            this.mortarIndicatorInstance.transform.localScale = Vector3.one * StaticValues.hermitMortarRadius;
            this.mortarIndicatorInstance.transform.localPosition = Vector3.zero;

            
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
}

