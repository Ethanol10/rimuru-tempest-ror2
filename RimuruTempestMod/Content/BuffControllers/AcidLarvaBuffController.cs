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
     Acid Larva 
     Effect: Springlike Limbs: Increased jump height
     */

    public class AcidLarvaBuffController : RimuruBaseBuffController
    {

        public override void Awake()
        {
            base.Awake();
            Hook();
            isPermaBuff = false;
            buffdef = Buffs.jumpHeightBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Springlike Limbs Skill</style> acquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_IncreaseJump;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
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

