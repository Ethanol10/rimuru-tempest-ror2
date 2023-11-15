using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using IL.RoR2;
using RimuruMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Bison
     Effect: Dash- pressing sprint dashes
     */

    public class BisonBuffController : RimuruBaseBuffController
    {
        private float buttonCooler;
        private RoR2.InputBankTest inputBank;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.dashBuff;
            inputBank = gameObject.GetComponent<RoR2.InputBankTest>();
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Dash Skill</style> acquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void Update()
        {

            //sprint to shunpo
            if (buttonCooler >= 0f)
            {
                buttonCooler -= Time.deltaTime * body.attackSpeed;

            }
            if (buttonCooler < 0f)
            {
                if (inputBank.sprint.justPressed)
                {
                    new SetDashStateMachine(body.masterObjectId).Send(NetworkDestination.Clients);
                    buttonCooler += 1f;
                }
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

