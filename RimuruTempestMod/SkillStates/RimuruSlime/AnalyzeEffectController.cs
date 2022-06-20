using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RimuruMod.Modules.Survivors;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace RimuruMod.SkillStates
{
    public class AnalyzeEffectController : MonoBehaviour
    {
        public CharacterBody charbody;
        private GameObject effectObj;

        public void Start()
        {
            charbody = this.gameObject.GetComponent<CharacterBody>();
            float charbodyheight = charbody.corePosition.y - charbody.footPosition.y;
            effectObj = Object.Instantiate<GameObject>(Modules.Assets.analyzeEffect, charbody.corePosition + Vector3.up * charbodyheight * 1.5f, Quaternion.LookRotation(charbody.characterDirection.forward));
            effectObj.transform.parent = charbody.gameObject.transform;
        }
        public void FixedUpdate()
        {
            if (charbody)
            {
                //If buff isn't present, destroy the effect and self.
                if (!charbody.HasBuff(Modules.Buffs.CritDebuff))
                {
                    Destroy(effectObj);
                    Destroy(this);
                }
            }

            if (!charbody)
            {
                Destroy(effectObj);
            }
        }

        public void OnDestroy()
        {
            Destroy(effectObj);
        }
    }
}