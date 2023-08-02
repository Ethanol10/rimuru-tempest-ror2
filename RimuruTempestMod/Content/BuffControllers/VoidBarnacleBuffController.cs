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

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Void Barnacle
     Effect: Gravity manipulation- enemies are pulled down
     */

    public class VoidBarnacleBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;
        private float pulseTimer;

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.gravManipulationBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Gravity Manipulation Skill</style> aquisition successful.");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if(body.HasBuff(Buffs.gravManipulationBuff))
            {
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
        }


        public void Hook()
        {

        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.gravManipulationBuff.buffIndex);
            }
        }

    }
}

