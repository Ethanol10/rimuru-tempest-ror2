using System;
using UnityEngine;
using RoR2;
using RimuruMod.Modules;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Vulture
     Effect: Flight- Hold space to fly, up to 3 seconds of height gain then glide
     */

    public class VultureBuffController : RimuruBaseBuffController
    {
        
        private float flightTimer;
        public bool flightExpired; // Used on the gliding to determine whether to allow him to glide.

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.flightBuff;
            flightExpired = false;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Flight Skill</style> acquisition successful.");
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
                            body.characterMotor.velocity = body.inputBank.aimDirection * body.moveSpeed * Modules.StaticValues.flightMoveSpeedMultiplier;
                            flightExpired = false;
                        }
                        else if (flightTimer >= StaticValues.flightBuffThreshold) 
                        {
                            flightExpired = true;
                        }
                    }

                    //move in the direction you're moving at a normal speed
                    //if (body.inputBank.moveVector != Vector3.zero)
                    //{
                    //    float yVelocity = body.characterMotor.velocity.y;
                    //    body.characterMotor.velocity = body.inputBank.moveVector * (body.moveSpeed);
                    //    body.characterMotor.velocity.y = yVelocity;
                    //    //body.characterMotor.rootMotion += body.inputBank.moveVector * body.moveSpeed * Time.fixedDeltaTime;
                    //    //body.characterMotor.disableAirControlUntilCollision = false;
                    //}
                    

                }
            }
            else if (body.characterMotor.isGrounded)
            {
                flightTimer = 0f;
                flightExpired = false;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


    }
}

