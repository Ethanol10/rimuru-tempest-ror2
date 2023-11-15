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
     Lunar Wisp 
     Effect: Crippling Blows: apply cripple on melee attacks
     */

    public class LunarWispBuffController : RimuruBaseBuffController
    {

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.crippleBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            RoR2.Chat.AddMessage("<style=cIsUtility>Crippling Blows Skill</style> acquisition successful.");
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

