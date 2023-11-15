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
using RimuruTempestMod.Content.BuffControllers;

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


		public bool isBodyInitialized;
		public bool devourShoot;


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
		}

		private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
		{
			orig.Invoke(self, damageReport);
			//devour check
			if (damageReport.attackerBody == characterBody)
			{
                if (damageReport.attackerBody && damageReport.victimBody)
				{
					if(damageReport.damageInfo.damage > 0 && damageReport.damageInfo.damageType == DamageType.BonusToLowHealth)
					{
						var name = BodyCatalog.GetBodyName(damageReport.victimBody.healthComponent.body.bodyIndex);
						GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);

						RimuruBaseBuffController incomingSkill;
                        Debug.Log("killed " + newbodyPrefab.name);
                        Debug.Log(Modules.StaticValues.rimDic[newbodyPrefab.name] + "skillname");
                        if (Modules.StaticValues.rimDic.ContainsKey(newbodyPrefab.name) && isBodyInitialized) 
						{
                            incomingSkill = Modules.StaticValues.rimDic[newbodyPrefab.name].Invoke(characterMaster);
                        }
                        AkSoundEngine.PostEvent("RimuruAnalyse", characterBody.gameObject);

                        DevourEffectController devourEffect;
                        if (!damageReport.attackerBody.gameObject.GetComponent<DevourEffectController>())
                        {
                            devourEffect = gameObject.AddComponent<DevourEffectController>();

                        }
                        else
                        {
                            devourEffect = gameObject.GetComponent<DevourEffectController>();

                        }
                        //Do something with incomingSkill I guess if necessary.
                    }
                }
			}
		}


		//Old function for reference.
        //private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        //{
        //    orig.Invoke(self, damageReport);
        //    //devour check
        //    if (damageReport.attackerBody?.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
        //    {
        //        if (damageReport.attackerBody && damageReport.victimBody)
        //        {
        //            if (damageReport.damageInfo.damage > 0 && damageReport.damageInfo.damageType == DamageType.BonusToLowHealth)
        //            {


        //                var name = BodyCatalog.GetBodyName(damageReport.victimBody.healthComponent.body.bodyIndex);
        //                GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);
        //                if (newbodyPrefab.name == "BeetleBody" ||
        //                    newbodyPrefab.name == "BeetleGuardBody" ||
        //                    newbodyPrefab.name == "BisonBody" ||
        //                    newbodyPrefab.name == "ParentBody" ||
        //                    newbodyPrefab.name == "VultureBody" ||
        //                    newbodyPrefab.name == "GupBody" ||
        //                    newbodyPrefab.name == "GipBody" ||
        //                    newbodyPrefab.name == "GeepBody")
        //                {
        //                    if (!damageReport.attackerBody.HasBuff(Buffs.strengthBuff))
        //                    {
        //                        Chat.AddMessage("<style=cIsUtility>Strengthen Body Skill</style> acquisition successful.");
        //                    }
        //                    SetEverythingFalse(damageReport.attackerBody);

        //                    damageReport.attackerBody.ApplyBuff(Buffs.strengthBuff.buffIndex, 1, -1);
        //                    strengthBuff = true;
        //                }

        //                if (newbodyPrefab.name == "LemurianBody" ||
        //                    //newbodyPrefab.name == "LemurianBruiserBody" ||
        //                    newbodyPrefab.name == "LunarExploderBody" ||
        //                    newbodyPrefab.name == "VerminBody" ||
        //                    newbodyPrefab.name == "GreaterWispBody" ||
        //                    newbodyPrefab.name == "WispBody" ||
        //                    newbodyPrefab.name == "MagmaWormBody")
        //                {
        //                    if (!damageReport.attackerBody.HasBuff(Buffs.fireBuff))
        //                    {
        //                        Chat.AddMessage("<style=cIsUtility>Fire Manipulation Skill</style> acquisition successful.");
        //                    }
        //                    SetEverythingFalse(damageReport.attackerBody);

        //                    damageReport.attackerBody.ApplyBuff(Buffs.fireBuff.buffIndex, 1, -1);
        //                    fireBuff = true;
        //                }

        //                if (
        //                    newbodyPrefab.name == "AcidLarvaBody" ||
        //                    newbodyPrefab.name == "BeetleQueen2Body" ||
        //                    newbodyPrefab.name == "FlyingVerminBody" ||
        //                    newbodyPrefab.name == "ClayGrenadierBody" ||
        //                    newbodyPrefab.name == "ClayBruiserBody" ||
        //                    newbodyPrefab.name == "ImpBody" ||
        //                    newbodyPrefab.name == "ImpBossBody" ||
        //                    newbodyPrefab.name == "ClayBossBody")
        //                {
        //                    if (!damageReport.attackerBody.HasBuff(Buffs.poisonMeleeBuff))
        //                    {
        //                        Chat.AddMessage("<style=cIsUtility>Poisonous Attacks Skill</style> acquisition successful.");
        //                    }
        //                    SetEverythingFalse(damageReport.attackerBody);

        //                    damageReport.attackerBody.ApplyBuff(Buffs.poisonMeleeBuff.buffIndex, 1, -1);
        //                    poisonMeleeBuff = true;
        //                }

        //                if (newbodyPrefab.name == "MiniMushroomBody" ||
        //                    newbodyPrefab.name == "VagrantBody" ||
        //                    newbodyPrefab.name == "VoidMegaCrabBody" ||
        //                    newbodyPrefab.name == "NullifierBody" ||
        //                    newbodyPrefab.name == "VoidJailerBody" ||
        //                    newbodyPrefab.name == "VoidBarnacleBody" ||
        //                    newbodyPrefab.name == "JellyfishBody")
        //                {
        //                    if (!damageReport.attackerBody.HasBuff(Buffs.ultraspeedRegenBuff))
        //                    {
        //                        Chat.AddMessage("<style=cIsUtility>Ultraspeed Regeneration Skill</style> acquisition successful.");
        //                    }
        //                    SetEverythingFalse(damageReport.attackerBody);

        //                    damageReport.attackerBody.ApplyBuff(Buffs.ultraspeedRegenBuff.buffIndex, 1, -1);
        //                    ultraspeedRegenBuff = true;
        //                }

        //                if (newbodyPrefab.name == "GolemBody" ||
        //                    newbodyPrefab.name == "BellBody" ||
        //                    newbodyPrefab.name == "HermitCrabBody" ||
        //                    newbodyPrefab.name == "TitanBody" ||
        //                    newbodyPrefab.name == "TitanGoldBody" ||
        //                    newbodyPrefab.name == "MinorConstructBody" ||
        //                    newbodyPrefab.name == "MinorConstructOnKillBody" ||
        //                    newbodyPrefab.name == "LunarGolemBody" ||
        //                    newbodyPrefab.name == "GravekeeperBody")
        //                {
        //                    if (!damageReport.attackerBody.HasBuff(Buffs.resistanceBuff))
        //                    {
        //                        Chat.AddMessage("<style=cIsUtility>Body Armor Skill</style> acquisition successful.");
        //                    }
        //                    SetEverythingFalse(damageReport.attackerBody);

        //                    damageReport.attackerBody.ApplyBuff(Buffs.resistanceBuff.buffIndex, 1, -1);
        //                    resistanceBuff = true;
        //                }

        //                if (newbodyPrefab.name == "MegaConstructBody" ||
        //                    newbodyPrefab.name == "RoboBallMiniBody" ||
        //                    newbodyPrefab.name == "RoboBallGreenBuddyBody" ||
        //                    newbodyPrefab.name == "RoboBallRedBuddyBody" ||
        //                    newbodyPrefab.name == "SuperRoboBallBossBody" ||
        //                    newbodyPrefab.name == "RoboBallBossBody" ||
        //                    newbodyPrefab.name == "ElectricWormBody" ||
        //                    newbodyPrefab.name == "LunarWispBody")
        //                {
        //                    if (!damageReport.attackerBody.HasBuff(Buffs.lightningBuff))
        //                    {
        //                        Chat.AddMessage("<style=cIsUtility>Lightning Manipulation Skill</style> acquisition successful.");
        //                    }
        //                    SetEverythingFalse(damageReport.attackerBody);

        //                    damageReport.attackerBody.ApplyBuff(Buffs.lightningBuff.buffIndex, 1, -1);
        //                    lightningBuff = true;
        //                }

        //            }
        //        }
        //    }
        //}
    }
}
