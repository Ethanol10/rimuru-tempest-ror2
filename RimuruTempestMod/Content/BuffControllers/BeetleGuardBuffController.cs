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
     Beetle Guard
     Effect: Melee Boost: 1.3 x Melee Damage
     */

    public class BeetleGuardBuffController : RimuruBaseBuffController
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
                body.AddBuff(Buffs.meleeBoostBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Melee Boost</style> aquisition successful.");
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.meleeBoostBuff.buffIndex);
            }
        }
    }
}

