using R2API.Networking;
using RimuruMod.Modules;
using System;
using UnityEngine;

namespace RimuruMod.Content.BuffControllers
{
    /*
     All Buffs should inherit from this.
     This should apply on the Master gameObject, not the body object.
     */

	public class RimuruBaseBuffController : MonoBehaviour
	{
        public bool isSkillOverride = false;
        public bool isPermaBuff = false;
        public float stopwatch;
        public float lifetime;
        public RoR2.CharacterBody body;
        public RoR2.CharacterMaster master;
        public RoR2.BuffDef buffdef;
        public RoR2.InputBankTest inputBank;

        public virtual void Awake()
        {
            ApplySkillChange();
            stopwatch = 0f;
            master = gameObject.GetComponent<RoR2.CharacterMaster>();
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();
        }

        public virtual void FixedUpdate()
        {
            if (body)
            {
                if (!body.HasBuff(buffdef))
                {
                    body.ApplyBuff(buffdef.buffIndex);
                }
            }

            if (!body) 
            {
                UpdateBody();
            }

            ActiveBuffEffect();
            stopwatch += Time.fixedDeltaTime;

            if (stopwatch >= lifetime && !isPermaBuff)
            {
                Destroy(this);
            }
        }

        public virtual void OnDestroy()
        {
            body.ApplyBuff(buffdef.buffIndex, 0);
        }


        public virtual void RefreshTimers()
        {
            lifetime += StaticValues.refreshTimerDuration;
        }

        public virtual void ActiveBuffEffect()
        {

        }

        public virtual void UpdateBody()
        {
            body = master.GetBody();
            inputBank = body.GetComponent<RoR2.InputBankTest>();
        }

        public virtual void ApplySkillChange()
        {
            //Apply skill overrides here.
        }
    }
}

