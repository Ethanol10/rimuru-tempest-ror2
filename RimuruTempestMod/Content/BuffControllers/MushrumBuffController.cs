using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using RimuruMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Mushrum
     Effect: Hyper regeneration- 1% max hp regen per second
     */

    public class MushrumBuffController : RimuruBaseBuffController
    {
        private float timer;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.hyperRegenBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Hyper Regeneration Skill</style> acquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > 1f)
            {
                timer = 0f;

                new HealNetworkRequest(body.masterObjectId, body.healthComponent.fullCombinedHealth * StaticValues.hyperRegenCoefficient).Send(NetworkDestination.Clients);
            }
        }


        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();
            //Apply skill overrides here.
        }
    }
}

