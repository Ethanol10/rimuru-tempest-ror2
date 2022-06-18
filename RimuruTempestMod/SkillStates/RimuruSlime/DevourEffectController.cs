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
    public class DevourEffectController : MonoBehaviour
    {
        public CharacterBody charbody;
        private GameObject effectObj;
        public float timer;

        public void Start()
        {
            charbody = this.gameObject.GetComponent<CharacterBody>();
            effectObj = Instantiate(Modules.Assets.devourskillgetEffect, charbody.corePosition + Vector3.up * 3f, Quaternion.LookRotation(charbody.characterDirection.forward));
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
            timer += Time.fixedDeltaTime;
            if (timer > 1.5f)
            {
                Destroy(effectObj);
                Destroy(this);
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