using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Networking;

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


		public bool alloyvulture;
		public bool alphacontruct;
		public bool beetle;
		public bool beetleguard;
		public bool blindpest;
		public bool blindvermin;
		public bool bison;
		public bool bronzong;
		public bool clayapothecary;
		public bool claytemplar;
		public bool greaterwisp;
		public bool gup;
		public bool hermitcrab;
		public bool imp;
		public bool jellyfish;
		public bool larva;
		public bool lemurian;
		public bool lesserwisp;
		public bool lunarexploder;
		public bool lunargolem;
		public bool lunarwisp;
		public bool minimushrum;
		public bool parent;
		public bool roboballminib;
		public bool stonegolem;
		public bool voidbarnacle;
		public bool voidjailer;
		public bool voidreaver;

		public bool beetlequeen;
		public bool claydunestrider;
		public bool grandparent;
		public bool grovetender;
		public bool impboss;
		public bool magmaworm;
		public bool overloadingworm;
		public bool scavenger;
		public bool soluscontrolunit;
		public bool stonetitan;
		public bool voiddevastator;
		public bool xiconstruct;



		public void Awake()
		{

			On.RoR2.CharacterBody.Start += CharacterBody_Start;
			On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
			//On.RoR2.Run.Start += Run_Start;
			alloyvulture = false;
			 alphacontruct = false;
			 beetle = false;
			 beetleguard = false;
			 blindpest = false;
			 blindvermin = false;
			 bison = false;
			 bronzong = false;
			 clayapothecary = false;
			 claytemplar = false;
			 greaterwisp = false;
			 gup = false;
			 hermitcrab = false;
			 imp = false;
			 jellyfish = false;
			 larva = false;
			 lemurian = false;
			 lesserwisp = false;
			 lunarexploder = false;
			 lunargolem = false;
			 lunarwisp = false;
			 minimushrum = false;
			 parent = false;
			 roboballminib = false;
			 stonegolem = false;
			 voidbarnacle = false;
			 voidjailer = false;
			 voidreaver = false;

			 beetlequeen = false;
			 claydunestrider = false;
			 grandparent = false;
			 grovetender = false;
			 impboss = false;
			 magmaworm = false;
			 overloadingworm = false;
			 scavenger = false;
			 soluscontrolunit = false;
			 stonetitan = false;
			 voiddevastator = false;
			 xiconstruct = false;

		}

		//public void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
		//{
		//	orig.Invoke(self);

		//	writeToSkillList(null, 0);
		//	writeToSkillList(null, 1);
		//	writeToSkillList(null, 2);
		//	writeToSkillList(null, 3);
		//	writeToSkillList(null, 4);
		//	writeToSkillList(null, 5);
		//	writeToSkillList(null, 6);
		//	writeToSkillList(null, 7);
		//}

		public void Start()
		{
			characterMaster = gameObject.GetComponent<CharacterMaster>();
			characterBody = characterMaster.GetBody();

			Rimurumastercon = characterMaster.gameObject.GetComponent<RimuruMasterController>();
			Rimurucon = characterBody.gameObject.GetComponent<RimuruController>();

			alloyvulture = false;
			alphacontruct = false;
			beetle = false;
			beetleguard = false;
			blindpest = false;
			blindvermin = false;
			bison = false;
			bronzong = false;
			clayapothecary = false;
			claytemplar = false;
			greaterwisp = false;
			gup = false;
			hermitcrab = false;
			imp = false;
			jellyfish = false;
			larva = false;
			lemurian = false;
			lesserwisp = false;
			lunarexploder = false;
			lunargolem = false;
			lunarwisp = false;
			minimushrum = false;
			parent = false;
			roboballminib = false;
			stonegolem = false;
			voidbarnacle = false;
			voidjailer = false;
			voidreaver = false;

			beetlequeen = false;
			claydunestrider = false;
			grandparent = false;
			grovetender = false;
			impboss = false;
			magmaworm = false;
			overloadingworm = false;
			scavenger = false;
			soluscontrolunit = false;
			stonetitan = false;
			voiddevastator = false;
			xiconstruct = false;
		}


		public void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
		{
			orig.Invoke(self);


			if (self.master.gameObject.GetComponent<RimuruMasterController>())
            {
				if(beetle == true)
                {
					self.AddBuff(Modules.Buffs.BeetleBuff);
                }
				if(lemurian == true)
                {
					self.AddBuff(Modules.Buffs.LemurianBuff);
                }

            }
        }


		public void FixedUpdate()
		{

		}

		private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
		{
			orig.Invoke(self, damageReport);
			//devour check
			if (damageReport.attackerBody.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
			{
				if (damageReport.attackerBody && damageReport.victimBody)
				{
					if (damageReport.damageInfo.damage > 0 && damageReport.damageInfo.damageType == DamageType.BonusToLowHealth)
					{
						var name = BodyCatalog.GetBodyName(damageReport.victimBody.healthComponent.body.bodyIndex);
						GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);
						if (newbodyPrefab.name == "BeetleBody")
						{
							Chat.AddMessage("<style=cIsUtility>Strengthen Body Skill</style> aquisition successful.");
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.BeetleBuff.buffIndex, 1, -1);

							beetle = true;
						}
						if (newbodyPrefab.name == "LemurianBody")
						{
							Chat.AddMessage("<style=cIsUtility>Fire Manipulation Skill</style> aquisition successful.");
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.LemurianBuff.buffIndex, 1, -1);
							lemurian = true;
						}
					}

					
				}


			}

		}

		public void SetEverythingFalse(CharacterBody characterBody)
		{
			//characterBody.ApplyBuff(Buffs.BeetleBuff.buffIndex, 0, 0);
			//characterBody.ApplyBuff(Buffs.LemurianBuff.buffIndex, 0, 0);

			characterBody.RemoveBuff(Buffs.BeetleBuff);
			characterBody.RemoveBuff(Buffs.LemurianBuff);

			alloyvulture = false;
			alphacontruct = false;
			beetle = false;
			beetleguard = false;
			blindpest = false;
			blindvermin = false;
			bison = false;
			bronzong = false;
			clayapothecary = false;
			claytemplar = false;
			greaterwisp = false;
			gup = false;
			hermitcrab = false;
			imp = false;
			jellyfish = false;
			larva = false;
			lemurian = false;
			lesserwisp = false;
			lunarexploder = false;
			lunargolem = false;
			lunarwisp = false;
			minimushrum = false;
			parent = false;
			roboballminib = false;
			stonegolem = false;
			voidbarnacle = false;
			voidjailer = false;
			voidreaver = false;

			beetlequeen = false;
			claydunestrider = false;
			grandparent = false;
			grovetender = false;
			impboss = false;
			magmaworm = false;
			overloadingworm = false;
			scavenger = false;
			soluscontrolunit = false;
			stonetitan = false;
			voiddevastator = false;
			xiconstruct = false;

		}
	}
}
