using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RimuruMod.Modules.Survivors;
using R2API.Networking;
using RimuruMod.Modules;
using System.Collections.Generic;
using System.Linq;

namespace RimuruMod.SkillStates
{
    public class Analyze : BaseSkillState
    {
        public RimuruController Rimurucon;
        public RimuruMasterController Rimurumastercon;
        public HurtBox Target;
        public bool aoe;

        public float duration = 0.1f;
        public override void OnEnter()
        {
            base.OnEnter();
            Rimurucon = base.GetComponent<RimuruController>();
            Rimurumastercon = characterBody.master.gameObject.GetComponent<RimuruMasterController>();

            if (Rimurucon && base.isAuthority)
            {
                Target = Rimurucon.GetTrackingTarget();
            }

            if (characterBody.HasBuff(Modules.Buffs.aoeBufferBuff))
            {
                aoe = true;
            }
            else
            {
                aoe = false;
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (aoe)
            {

                BullseyeSearch search = new BullseyeSearch
                {

                    teamMaskFilter = TeamMask.GetEnemyTeams(characterBody.teamComponent.teamIndex),
                    filterByLoS = false,
                    searchOrigin = characterBody.corePosition,
                    searchDirection = UnityEngine.Random.onUnitSphere,
                    sortMode = BullseyeSearch.SortMode.Distance,
                    maxDistanceFilter = StaticValues.aoeBufferRadius,
                    maxAngleFilter = 360f
                };

                search.RefreshCandidates();
                search.FilterOutGameObject(characterBody.gameObject);

                List<HurtBox> target = search.GetResults().ToList<HurtBox>();
                foreach (HurtBox singularTarget in target)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        //add the debuff to all enemies
                        singularTarget.healthComponent.body.ApplyBuff(Buffs.CritDebuff.buffIndex, Config.analyseDebuffduration.Value);

                    }
                }
            }
            else
            {
                if (!Target)
                {
                    base.skillLocator.utility.AddOneStock();
                    return;

                }
                if (Target)
                {
                    Target.healthComponent.body.ApplyBuff(Buffs.CritDebuff.buffIndex, 1, Config.analyseDebuffduration.Value);
                    AkSoundEngine.PostEvent("RimuruAnalyse", base.gameObject);
                }

            }


        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}