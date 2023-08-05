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

    public class AcidLarvaBuffController : RimuruBaseBuffController
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
                body.AddBuff(Buffs.jumpHeightBuff.buffIndex);
            }

            RoR2.Chat.AddMessage("<style=cIsUtility>Springlike Limbs</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_IncreaseJump;
        }

        public void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.jumpHeightBuff.buffIndex);
            }
        }

        public void CharacterBody_IncreaseJump(On.RoR2.CharacterBody.orig_FixedUpdate orig, RoR2.CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(Buffs.jumpHeightBuff) && self.inputBank.jump.down)
                {
                    self.characterMotor.velocity.y -= Time.fixedDeltaTime * Physics.gravity.y * 0.5f;
                }
            }
        }
    }
}

