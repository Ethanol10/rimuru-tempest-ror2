using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruTempestMod.Modules.Networking;
using IL.RoR2;
using System.Collections.Generic;
using BullseyeSearch = RoR2.BullseyeSearch;
using static UnityEngine.ParticleSystem.PlaybackState;
using R2API.Networking.Interfaces;
using UnityEngine.Networking;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Vulture
     Effect: Flight- Hold space to fly, up to 3 seconds of height gain then glide
     */

    public class VultureBuffController : RimuruBaseBuffController
    {
        
        private float flightTimer;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.flightBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Flight Skill</style> aquisition successful.");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //air walk
            if (!body.characterMotor.isGrounded)
            {
                flightTimer += Time.fixedDeltaTime;
                //after 0.5 seconds start flying
                if (flightTimer > 0.5f)
                {
                        if (body.inputBank.jump.down)
                        {
                            //before flight timer runs out, can rise regardless besides while using a skill
                            if (flightTimer < StaticValues.flightBuffThreshold)
                            {
                                if (body.inputBank.skill1.down
                                | body.inputBank.skill2.down
                                | body.inputBank.skill3.down
                                | body.inputBank.skill4.down)
                                {
                                    body.characterMotor.velocity.y = 0f;
                                }
                                else
                                {
                                    body.characterMotor.velocity.y = body.moveSpeed;
                                }
                            }
                            //after airwalk timer, need to ensure not holding any skill or any move direction to rise
                            else if (flightTimer >= StaticValues.flightBuffThreshold)
                            {
                                if (body.inputBank.skill1.down
                                | body.inputBank.skill2.down
                                | body.inputBank.skill3.down
                                | body.inputBank.skill4.down)
                                {
                                    body.characterMotor.velocity.y = 0f;
                                }

                                if (body.inputBank.moveVector == Vector3.zero)
                                {
                                    body.characterMotor.velocity.y = body.moveSpeed;
                                }
                                else
                                {
                                    body.characterMotor.velocity.y = 0f;
                                }
                            }


                        }

                        //move in the direction you're moving at a normal speed
                        if (body.inputBank.moveVector != Vector3.zero)
                        {
                            //body.characterMotor.velocity = body.inputBank.moveVector * (body.moveSpeed);
                            body.characterMotor.rootMotion += body.inputBank.moveVector * body.moveSpeed * Time.fixedDeltaTime;
                            //body.characterMotor.disableAirControlUntilCollision = false;
                        }
                    

                }
            }
            else if (body.characterMotor.isGrounded)
            {
                flightTimer = 0f;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


    }
}

