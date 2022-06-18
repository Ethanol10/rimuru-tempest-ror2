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
    public class WetEffectController : MonoBehaviour
    {
        public CharacterBody charbody;
        private GameObject effectObj;

        public void Start()
        {
            charbody = this.gameObject.GetComponent<CharacterBody>();
            effectObj = Instantiate(Modules.Assets.wetEffect, charbody.corePosition + Vector3.up * 2f, Quaternion.LookRotation(charbody.characterDirection.forward));
        }

        public void Update()
        {
            //Handle transform of effectObj
            if (effectObj)
            {
                effectObj.transform.position = charbody.corePosition + Vector3.up * 1f;
                effectObj.transform.rotation = Quaternion.LookRotation(charbody.characterDirection.forward);
            }
        }
        public void FixedUpdate()
        {
            if (charbody)
            {
                //If buff isn't present, destroy the effect and self.
                if (!charbody.HasBuff(Modules.Buffs.WetDebuff))
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