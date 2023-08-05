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

        public virtual void Awake()
        {
            ApplySkillChange();
            stopwatch = 0f;
            lifetime = 0f;
            RefreshTimers(); 
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

        public virtual void ApplySkillChange()
        {
            //Apply skill overrides here.
        }
    }
}

