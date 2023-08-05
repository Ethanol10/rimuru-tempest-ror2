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
     Gup
     Effect: Gluttony - Increase devour range
     */

    public class GupBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

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
                body.AddBuff(Buffs.devourBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Gluttony</style> aquisition successful.");
        }
        
        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.devourBuff.buffIndex);
            }
        }
    }
}

