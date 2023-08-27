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
     Magma Worm
     Effect: Fire Manipulation: Melee Attacks Apply Fire
     */

    public class OverloadingWormBuffController : RimuruBaseBuffController
    {
        
        RoR2.BlastAttack blastAttack;
        float timer;

        public override void Awake()
        {
            base.Awake();
            isPermaBuff = false;
            buffdef = Buffs.lightningPulseBuff;
        }

        public void Start()
        {
            timer = 0;
            blastAttack = new RoR2.BlastAttack();
            blastAttack.radius = 10f;
            blastAttack.procCoefficient = 0;
            blastAttack.position = body.transform.position;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = RoR2.Util.CheckRoll(body.crit);
            blastAttack.baseDamage = body.damage;
            blastAttack.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            blastAttack.teamIndex = RoR2.TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Shock5s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;
            body = gameObject.GetComponent<RoR2.CharacterMaster>().GetBody();


            RoR2.Chat.AddMessage("<style=cIsUtility>Lightning Manipulation</style> aquisition successful.");
        }

        public void Update()
        {
            timer = Time.deltaTime;

            if (timer >= 2)
            {
                timer = 0;
                blastAttack.Fire();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
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

