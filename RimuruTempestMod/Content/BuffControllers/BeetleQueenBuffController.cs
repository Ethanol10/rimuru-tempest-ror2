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
     Acid Larva 
     Effect: Springlike Limbs: Increased jump height
     */

    public class BeetleQueenBuffController : RimuruBaseBuffController
    {
        public RoR2.CharacterBody body;

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = true;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            if (body)
            {
                body.AddBuff(Buffs.tripleWaterBladeBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Triple Waterblade</style> aquisition successful.");
        }

        public void Hook()
        {
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.tripleWaterBladeBuff.buffIndex);
            }
        }
    }
}

