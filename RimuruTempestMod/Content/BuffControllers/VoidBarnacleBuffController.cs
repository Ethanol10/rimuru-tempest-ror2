using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruMod.Modules.Networking;
using IL.RoR2;
using System.Collections.Generic;
using BullseyeSearch = RoR2.BullseyeSearch;
using static UnityEngine.ParticleSystem.PlaybackState;
using R2API.Networking.Interfaces;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Void Barnacle
     Effect: Gravity manipulation- enemies are pulled down
     */

    public class VoidBarnacleBuffController : RimuruBaseBuffController
    {
        
        private float pulseTimer;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.gravManipulationBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            RoR2.Chat.AddMessage("<style=cIsUtility>Gravity Manipulation Skill</style> acquisition successful.");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(pulseTimer < 1f)
            {
                pulseTimer += Time.fixedDeltaTime;
            }
            else if (pulseTimer >= 1f)
            {
                pulseTimer = 0f;

                new PeformDirectionalForceNetworkRequest(body.masterObjectId, Vector3.down, StaticValues.gravManipulationForce, body.damage * StaticValues.gravManipulationDamageCoefficient, StaticValues.gravManipulationRadius).Send(NetworkDestination.Clients);
            }
            
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}

