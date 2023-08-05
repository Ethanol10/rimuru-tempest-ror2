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
     Flying Vermin aka Blind Pest
     Effect: AOE buffer- analyze analzyes multiple enimes
     */

    public class FlyingVerminBuffController : RimuruBaseBuffController
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
                body.AddBuff(Buffs.aoeBufferBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>AOE Buffer Skill</style> aquisition successful.");
        }

        public void Hook()
        {

        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.aoeBufferBuff.buffIndex);
            }
        }

    }
}

