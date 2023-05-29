﻿using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Alpha Construct
     Effect: Hastening - Attack Speed x 1.2
     */

    public class AlphaConstructBuffController : RimuruBaseBuffController
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
                body.AddBuff(Buffs.bleedMeleeBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Bloody Edge Skill</style> aquisition successful.");
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.bleedMeleeBuff.buffIndex);
            }
        }
    }
}
