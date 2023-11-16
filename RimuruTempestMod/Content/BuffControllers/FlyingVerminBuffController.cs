using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;

namespace RimuruMod.Content.BuffControllers
{
    /*
     Flying Vermin aka Blind Pest
     Effect: AOE buffer - analyze analzyes multiple enemies
     */

    public class FlyingVerminBuffController : RimuruBaseBuffController
    {
        


        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.aoeBufferBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>AOE Buffer Skill</style> acquisition successful.");
        }

        public void Hook()
        {

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
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

