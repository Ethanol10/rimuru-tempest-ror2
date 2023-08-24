using System;
using UnityEngine;
using RoR2;
using On.RoR2;
using RimuruMod.Modules;
using System.Reflection;
using R2API.Networking;
using UnityEngine.Networking;

namespace RimuruTempestMod.Content.BuffControllers
{
    /*
     Lunar Exploder
     Effect: Luck manipulation- get 1 luck
     */

    public class LunarExploderBuffController : RimuruBaseBuffController
    {
        

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
                body.AddBuff(Buffs.lunarExploderLuckManipulationBuff.buffIndex);
            }
            body.master.luck += StaticValues.luckAmount;

            RoR2.Chat.AddMessage("<style=cIsUtility>Luck Manipulation Skill</style> aquisition successful.");
        }

        public void Hook()
        {
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
        }


        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, RoR2.CharacterMaster self)
        {
            orig.Invoke(self);

            if (self)
            {
                if (body.masterObjectId == self.netId)
                {
                    self.luck = 0;
                    if(body.HasBuff(Buffs.lunarExploderLuckManipulationBuff))
                    {
                        self.luck += StaticValues.luckAmount;
                    }
                    self.luck += self.inventory.GetItemCount(RoR2.RoR2Content.Items.Clover);
                    self.luck -= self.inventory.GetItemCount(RoR2.RoR2Content.Items.LunarBadLuck);

                }

                
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!body.HasBuff(Buffs.bleedMeleeBuff))
            {
                body.ApplyBuff(Buffs.bleedMeleeBuff.buffIndex);
            }
        }

        public override void OnDestroy()
        {
            if (body)
            {
                body.RemoveBuff(Buffs.lunarExploderLuckManipulationBuff.buffIndex);
                body.master.luck -= StaticValues.luckAmount;
            }
            On.RoR2.CharacterMaster.OnInventoryChanged -= CharacterMaster_OnInventoryChanged;
        }

        public override void RefreshTimers()
        {
            base.RefreshTimers();
        }

        public override void ActiveBuffEffect()
        {
            body.AddBuff(Buffs.lunarExploderLuckManipulationBuff.buffIndex);
        }

        public override void ApplySkillChange()
        {
            base.ApplySkillChange();   
            //Apply skill overrides here.
        }
    }
}

