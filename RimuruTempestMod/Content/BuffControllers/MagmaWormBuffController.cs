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
     Magma Worm
     Effect: Fire Manipulation: Melee Attacks Apply Fire
     */

    public class MagmaWormBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = true;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.fireMeleeBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Fire Manipulation</style> aquisition successful.");
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.fireMeleeBuff.buffIndex);
            }
        }
    }
}

