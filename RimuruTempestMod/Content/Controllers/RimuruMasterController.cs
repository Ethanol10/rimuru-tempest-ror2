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

		//buff checks
		public bool strengthBuff;
		public bool fireBuff;
		public bool lightningBuff;
		public bool resistanceBuff;
		public bool ultraspeedRegenBuff;
		public bool poisonMeleeBuff;

		//buffs
		public float regenTimer;
        public float regenAmount;
        public float shockTimer;

		public bool isBodyInitialized;
		public bool devourShoot;


		public void Awake()
		{
			isBodyInitialized = false;
			On.RoR2.CharacterBody.Start += CharacterBody_Start;
			On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
			//alloyvulture = false;
			// alphacontruct = false;
			// beetle = false;
			// beetleguard = false;
			// blindpest = false;
			// blindvermin = false;
			// bison = false;
			// bronzong = false;
			// clayapothecary = false;
			// claytemplar = false;
			// greaterwisp = false;
			// gup = false;
			// hermitcrab = false;
			// imp = false;
			// jellyfish = false;
			// larva = false;
			// lemurian = false;
			// lesserwisp = false;
			// lunarexploder = false;
			// lunargolem = false;
			// lunarwisp = false;
			// minimushrum = false;
			// parent = false;
			// roboballminib = false;
			// stonegolem = false;
			// voidbarnacle = false;
			// voidjailer = false;
			// voidreaver = false;

			// beetlequeen = false;
			// claydunestrider = false;
			// grandparent = false;
			// grovetender = false;
			// impboss = false;
			// magmaworm = false;
			// overloadingworm = false;
			// scavenger = false;
			// soluscontrolunit = false;
			// stonetitan = false;
			// voiddevastator = false;
			// xiconstruct = false;
			strengthBuff = false;
			fireBuff = false;
			lightningBuff = false;
			resistanceBuff = false;
			ultraspeedRegenBuff = false;
			poisonMeleeBuff = false;

		}

		public void OnDestroy()
		{
			On.RoR2.CharacterBody.Start -= CharacterBody_Start;
			On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
		}

		public void Start()
		{
			characterMaster = gameObject.GetComponent<CharacterMaster>();

			Rimurumastercon = characterMaster.gameObject.GetComponent<RimuruMasterController>();

			this.devourShoot = false;

			//alloyvulture = false;
			//alphacontruct = false;
			//beetle = false;
			//beetleguard = false;
			//blindpest = false;
			//blindvermin = false;
			//bison = false;
			//bronzong = false;
			//clayapothecary = false;
			//claytemplar = false;
			//greaterwisp = false;
			//gup = false;
			//hermitcrab = false;
			//imp = false;
			//jellyfish = false;
			//larva = false;
			//lemurian = false;
			//lesserwisp = false;
			//lunarexploder = false;
			//lunargolem = false;
			//lunarwisp = false;
			//minimushrum = false;
			//parent = false;
			//roboballminib = false;
			//stonegolem = false;
			//voidbarnacle = false;
			//voidjailer = false;
			//voidreaver = false;

			//beetlequeen = false;
			//claydunestrider = false;
			//grandparent = false;
			//grovetender = false;
			//impboss = false;
			//magmaworm = false;
			//overloadingworm = false;
			//scavenger = false;
			//soluscontrolunit = false;
			//stonetitan = false;
			//voiddevastator = false;
			//xiconstruct = false;
		}


		public void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
		{
			orig.Invoke(self);


			if (Rimurumastercon)
            {
				if(strengthBuff == true)
                {
					self.AddBuff(Modules.Buffs.strengthBuff);
                }
				if(fireBuff == true)
                {
					self.AddBuff(Modules.Buffs.fireBuff);
				}
				if (lightningBuff == true)
				{
					self.AddBuff(Modules.Buffs.lightningBuff);
				}
				if (ultraspeedRegenBuff == true)
				{
					self.AddBuff(Modules.Buffs.ultraspeedRegenBuff);
				}
				if (resistanceBuff == true)
				{
					self.AddBuff(Modules.Buffs.resistanceBuff);
				}
				if (poisonMeleeBuff == true)
				{
					self.AddBuff(Modules.Buffs.poisonMeleeBuff);
				}

			}
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
			else 
			{
                if (characterBody.HasBuff(Buffs.ultraspeedRegenStackBuff))
                {
                    if (regenTimer > 1f)
                    {
                        int buffcount = characterBody.GetBuffCount(Buffs.ultraspeedRegenStackBuff);
                        if (buffcount > 1)
                        {
                            if (buffcount >= 2)
                            {
                                regenTimer = 0;
                                characterBody.RemoveBuff(Modules.Buffs.ultraspeedRegenStackBuff.buffIndex);
                                characterBody.healthComponent.Heal(regenAmount / StaticValues.ultraspeedBuffStacks, new ProcChainMask(), true);
                            }
                        }
                        else
                        {
                            characterBody.RemoveBuff(Modules.Buffs.ultraspeedRegenStackBuff.buffIndex);
                            characterBody.healthComponent.Heal(regenAmount / StaticValues.ultraspeedBuffStacks, new ProcChainMask(), true);
                        }
                    }
                    else
                    {
                        regenTimer += Time.fixedDeltaTime;
                    }
                }
                if (characterBody.HasBuff(Buffs.lightningBuff))
                {
                    if (shockTimer > StaticValues.lightningPulseTimer)
                    {
                        shockTimer = 0;
                        EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova"), new EffectData
                        {
                            origin = characterBody.corePosition,
                            scale = Modules.Config.blackLightningRadius.Value * characterBody.attackSpeed / 2
                        }, true);

                        new BlastAttack
                        {
                            attacker = characterBody.gameObject,
                            teamIndex = TeamComponent.GetObjectTeam(characterBody.gameObject),
                            falloffModel = BlastAttack.FalloffModel.None,
                            baseDamage = characterBody.damage * Modules.Config.blackLightningDamageCoefficient.Value,
                            damageType = DamageType.Shock5s,
                            damageColorIndex = DamageColorIndex.WeakPoint,
                            baseForce = 0,
                            position = characterBody.transform.position,
                            radius = Modules.Config.blackLightningRadius.Value * characterBody.attackSpeed / 2,
                            procCoefficient = 1f,
                            attackerFiltering = AttackerFiltering.NeverHitSelf,
                        }.Fire();
                    }
                    else
                    {
                        shockTimer += Time.fixedDeltaTime;
                    }
                }
            }
		}

		private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
		{
			orig.Invoke(self, damageReport);
			//devour check
			if (damageReport.attackerBody?.baseNameToken == RimuruPlugin.DEVELOPER_PREFIX + "_RIMURUSLIME_BODY_NAME")
			{
                if (damageReport.attackerBody && damageReport.victimBody)
				{
					if(damageReport.damageInfo.damage > 0 && damageReport.damageInfo.damageType == DamageType.BonusToLowHealth)
					{
						var name = BodyCatalog.GetBodyName(damageReport.victimBody.healthComponent.body.bodyIndex);
						GameObject newbodyPrefab = BodyCatalog.FindBodyPrefab(name);
						if (newbodyPrefab.name == "BeetleBody" ||
							newbodyPrefab.name == "BeetleGuardBody" ||
							newbodyPrefab.name == "BisonBody" ||
							newbodyPrefab.name == "ParentBody" ||
							newbodyPrefab.name == "VultureBody" ||
							newbodyPrefab.name == "GupBody" ||
							newbodyPrefab.name == "GipBody" ||
							newbodyPrefab.name == "GeepBody")
						{
                            if (!damageReport.attackerBody.HasBuff(Buffs.strengthBuff))
							{
								Chat.AddMessage("<style=cIsUtility>Strengthen Body Skill</style> aquisition successful.");
							}
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.strengthBuff.buffIndex, 1, -1);
							strengthBuff = true;
						}

						if (newbodyPrefab.name == "LemurianBody" ||
							newbodyPrefab.name == "LemurianBruiserBody" ||
							newbodyPrefab.name == "LunarExploderBody" ||
							newbodyPrefab.name == "VerminBody" ||
							newbodyPrefab.name == "GreaterWispBody" ||
							newbodyPrefab.name == "WispBody" ||
							newbodyPrefab.name == "MagmaWormBody")
						{
							if (!damageReport.attackerBody.HasBuff(Buffs.fireBuff))
							{
								Chat.AddMessage("<style=cIsUtility>Fire Manipulation Skill</style> aquisition successful.");
							}
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.fireBuff.buffIndex, 1, -1);
							fireBuff = true;
						}

						if (
							newbodyPrefab.name == "AcidLarvaBody" ||
							newbodyPrefab.name == "BeetleQueen2Body" ||
							newbodyPrefab.name == "FlyingVerminBody" ||
							newbodyPrefab.name == "ClayGrenadierBody" ||
							newbodyPrefab.name == "ClayBruiserBody" ||
							newbodyPrefab.name == "ImpBody" ||
							newbodyPrefab.name == "ImpBossBody" ||
							newbodyPrefab.name == "ClayBossBody")
						{
							if (!damageReport.attackerBody.HasBuff(Buffs.poisonMeleeBuff))
							{
								Chat.AddMessage("<style=cIsUtility>Poisonous Attacks Skill</style> aquisition successful.");
							}
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.poisonMeleeBuff.buffIndex, 1, -1);
							poisonMeleeBuff = true;
						}

						if (newbodyPrefab.name == "MiniMushroomBody" ||
							newbodyPrefab.name == "VagrantBody" ||
							newbodyPrefab.name == "VoidMegaCrabBody" ||
							newbodyPrefab.name == "NullifierBody" ||
							newbodyPrefab.name == "VoidJailerBody" ||
							newbodyPrefab.name == "VoidBarnacleBody" ||
							newbodyPrefab.name == "JellyfishBody")
						{
							if (!damageReport.attackerBody.HasBuff(Buffs.ultraspeedRegenBuff))
							{
								Chat.AddMessage("<style=cIsUtility>Ultraspeed Regeneration Skill</style> aquisition successful.");
							}
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.ultraspeedRegenBuff.buffIndex, 1, -1);
							ultraspeedRegenBuff = true;
						}

						if (newbodyPrefab.name == "GolemBody" ||
							newbodyPrefab.name == "BellBody" ||
							newbodyPrefab.name == "HermitCrabBody" ||
							newbodyPrefab.name == "TitanBody" ||
							newbodyPrefab.name == "TitanGoldBody" ||
							newbodyPrefab.name == "MinorConstructBody" ||
							newbodyPrefab.name == "MinorConstructOnKillBody" ||
							newbodyPrefab.name == "LunarGolemBody" ||
							newbodyPrefab.name == "GravekeeperBody")
						{
							if (!damageReport.attackerBody.HasBuff(Buffs.resistanceBuff))
							{
								Chat.AddMessage("<style=cIsUtility>Body Armor Skill</style> aquisition successful.");
							}
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.resistanceBuff.buffIndex, 1, -1);
							resistanceBuff = true;
						}

						if (newbodyPrefab.name == "MegaConstructBody" ||
							newbodyPrefab.name == "RoboBallMiniBody" ||
							newbodyPrefab.name == "RoboBallGreenBuddyBody" ||
							newbodyPrefab.name == "RoboBallRedBuddyBody" ||
							newbodyPrefab.name == "SuperRoboBallBossBody" ||
							newbodyPrefab.name == "RoboBallBossBody" ||
							newbodyPrefab.name == "ElectricWormBody" ||
							newbodyPrefab.name == "LunarWispBody")
						{
							if (!damageReport.attackerBody.HasBuff(Buffs.lightningBuff))
							{
								Chat.AddMessage("<style=cIsUtility>Lightning Manipulation Skill</style> aquisition successful.");
							}
							SetEverythingFalse(damageReport.attackerBody);

							damageReport.attackerBody.ApplyBuff(Buffs.lightningBuff.buffIndex, 1, -1);
							lightningBuff = true;
						}

					}
				}
			}
		}

		public void SetEverythingFalse(CharacterBody characterBody)
		{

			DevourEffectController controller = characterBody.gameObject.GetComponent<DevourEffectController>();
			if (!controller)
			{
				controller = characterBody.gameObject.AddComponent<DevourEffectController>();
				controller.charbody = characterBody;
			}

			characterBody.RemoveBuff(Buffs.strengthBuff);
			characterBody.RemoveBuff(Buffs.fireBuff);
			characterBody.RemoveBuff(Buffs.resistanceBuff);
			characterBody.RemoveBuff(Buffs.ultraspeedRegenBuff);
			characterBody.RemoveBuff(Buffs.poisonMeleeBuff);
			characterBody.RemoveBuff(Buffs.lightningBuff);


			strengthBuff = false;
			fireBuff = false;
			lightningBuff = false;
			resistanceBuff = false;
			ultraspeedRegenBuff = false;
			poisonMeleeBuff = false;
			//alloyvulture = false;
			//alphacontruct = false;
			//beetle = false;
			//beetleguard = false;
			//blindpest = false;
			//blindvermin = false;
			//bison = false;
			//bronzong = false;
			//clayapothecary = false;
			//claytemplar = false;
			//greaterwisp = false;
			//gup = false;
			//hermitcrab = false;
			//imp = false;
			//jellyfish = false;
			//larva = false;
			//lemurian = false;
			//lesserwisp = false;
			//lunarexploder = false;
			//lunargolem = false;
			//lunarwisp = false;
			//minimushrum = false;
			//parent = false;
			//roboballminib = false;
			//stonegolem = false;
			//voidbarnacle = false;
			//voidjailer = false;
			//voidreaver = false;

			//beetlequeen = false;
			//claydunestrider = false;
			//grandparent = false;
			//grovetender = false;
			//impboss = false;
			//magmaworm = false;
			//overloadingworm = false;
			//scavenger = false;
			//soluscontrolunit = false;
			//stonetitan = false;
			//voiddevastator = false;
			//xiconstruct = false;

		}
	}
}
