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
     Clay Dunestrider 
     Effect: Tar manipulation- enemies get slowed/attackspeed slow nearby
     */

    public class ClayDunestriderBuffController : RimuruBaseBuffController
    {
        private GameObject tarManipIndicator;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.tarManipBuff;
        }

        public void Start()
        {
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();

            RoR2.Chat.AddMessage("<style=cIsUtility>Tar Manipulation Skill</style> aquisition successful.");
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            
        }

        public void Update()
        {
            if (!this.tarManipIndicator)
            {
                CreateTarManipIndicator();
            }
            if (tarManipIndicator)
            {
                this.tarManipIndicator.transform.parent = body.transform;
                this.tarManipIndicator.transform.localScale = Vector3.one * StaticValues.tarManipRadius;
                this.tarManipIndicator.transform.localPosition = body.corePosition;

            }

        }

        public void CreateTarManipIndicator()
        {
            this.tarManipIndicator = UnityEngine.Object.Instantiate<GameObject>(Assets.tarManipIndicatorPrefab);
            this.tarManipIndicator.SetActive(true);

            this.tarManipIndicator.transform.localScale = Vector3.one * StaticValues.tarManipRadius;
            this.tarManipIndicator.transform.localPosition = body.corePosition;


        }

        public override void OnDestroy()
        {
            base.FixedUpdate();
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

