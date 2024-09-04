using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Networking;
using RimuruMod.SkillStates;
using RimuruMod.Content.BuffControllers;
using UnityEngine.PlayerLoop;
using RimuruMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace RimuruMod.Modules.Survivors
{
	[RequireComponent(typeof(CharacterBody))]
	[RequireComponent(typeof(TeamComponent))]
	[RequireComponent(typeof(InputBankTest))]
	public class RimuruMasterController : MonoBehaviour
	{
		string prefix = RimuruSlime.RIMURU_PREFIX;

		public RimuruMasterController Rimurumastercon;
		public RimuruController Rimurucon;
		private CharacterMaster characterMaster;
		private CharacterBody characterBody;
		private HealthComponent healthComponent;

		public bool isBodyInitialized;
		public bool devourShoot;

		public bool setHealthToValue;
		public float oldHealth;


		public void Awake()
		{
			isBodyInitialized = false;
			On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;

        }

		public void OnDestroy()
		{
			On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
		}

		public void Start()
		{
			characterMaster = gameObject.GetComponent<CharacterMaster>();
            characterBody = characterMaster.GetBody();
            Rimurumastercon = characterMaster.gameObject.GetComponent<RimuruMasterController>();

			this.devourShoot = false;
			this.setHealthToValue = false;

		}



		public void FixedUpdate()
		{

			if (!isBodyInitialized)
			{
				characterBody = characterMaster.GetBody();
				if (characterBody)
				{
					Rimurucon = characterBody.GetComponent<RimuruController>();
					isBodyInitialized = true;
				}
			}

			if (setHealthToValue) 
			{
				if (!healthComponent) 
				{
					healthComponent = characterMaster.GetBody().healthComponent;
				}

				if (healthComponent) 
				{
                    new UpdateControllers(characterMaster.netId, oldHealth).Send(NetworkDestination.Clients);

					//Accounting for regen I guess.
					if (healthComponent.Networkhealth < oldHealth + 5f) 
					{
						setHealthToValue = false;
					}
                }
            }
		}

		private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
		{
			orig.Invoke(self, damageReport);
			//devour check
			if (damageReport.attackerBody == characterBody)
			{
                if (damageReport.attackerBody && damageReport.victimBody)
				{
					if(damageReport.damageInfo.damage > 0 && damageReport.victimBody.HasBuff(Buffs.rimuruDevourDebuff) && damageReport.attackerBody.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
					{
						var name = BodyCatalog.GetBodyName(damageReport.victimBody.healthComponent.body.bodyIndex);
						GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);

						RimuruBaseBuffController incomingSkill;
                        //Debug.Log("killed " + newbodyPrefab.name);
                        //Debug.Log(Modules.StaticValues.rimDic[newbodyPrefab.name] + "skillname");
                        if (Modules.StaticValues.rimDic.ContainsKey(newbodyPrefab.name) && isBodyInitialized) 
						{
                            incomingSkill = Modules.StaticValues.rimDic[newbodyPrefab.name].Invoke(characterMaster);
                        }
                        AkSoundEngine.PostEvent("RimuruAnalyse", characterBody.gameObject);

                        RoR2.EffectManager.SpawnEffect(Modules.AssetsRimuru.devourskillgetEffect, new RoR2.EffectData
                        {
                            origin = damageReport.victimBody.corePosition + Vector3.up * 2f,
                            scale = 1f,
                            rotation = Quaternion.identity,

                        }, true);
                        //Do something with incomingSkill I guess if necessary.
                    }
                }
			}
		}
    }
}
