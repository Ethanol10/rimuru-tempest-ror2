using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Jellyfish
     Effect: Conductivity - Waterblade additionally deals shock damage 
     */

    public class JellyfishBuffController : RimuruBaseBuffController
    {
        

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.ConductivityBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Conductivity</style> aquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
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
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}

