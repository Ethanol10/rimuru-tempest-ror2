using RimuruMod.Modules;
using System;
using UnityEngine;

namespace RimuruTempestMod.Content.BuffControllers
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

        public virtual void Awake()
        {
            ApplySkillChange();
            stopwatch = 0f;
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();
        }

        public virtual void FixedUpdate()
        {
            ActiveBuffEffect();
            stopwatch += Time.fixedDeltaTime;
            

            if (stopwatch >= lifetime && !isPermaBuff)
            {
                Destroy(this);
            }
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
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();
        }

        public virtual void ApplySkillChange()
        {
            //Apply skill overrides here.
        }
    }
}

