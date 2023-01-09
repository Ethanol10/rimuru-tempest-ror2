using EntityStates;
using RoR2;
using UnityEngine;
using RimuruMod.Modules.Survivors;
using UnityEngine.Networking;
using RoR2.Audio;
using static RoR2.BulletAttack;

namespace RimuruMod.SkillStates
{
    public class BlackLightning : BaseSkillState
    {
        public RimuruController Rimurucon;
        private DamageType damageType;
        private Ray aimRay;


        private bool beamPlay;
        private float basefireInterval = 0.25f;
        private float fireInterval;
        private float basetotalDuration = Modules.Config.blackLightningTotalDuration.Value;
        private float totalDuration;
        private float range = 60f;
        private float damageCoefficient = Modules.Config.blackLightningDamageCoefficient.Value;
        private float procCoefficient = Modules.Config.blackLightningProcCoefficient.Value;
        private float force = 100f;
        private float fireTimer;
        public string muzzleString = "LWrist";
        public uint loopID;

        private GameObject blacklightning = UnityEngine.Object.Instantiate(Modules.Assets.blacklightning);
        private ParticleSystem mainBlacklightning;
        private BulletAttack attack;
        private BlastAttack blastAttack;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            updateAimRay();
            base.characterBody.SetAimTimer(2f);
            totalDuration = basetotalDuration;
            fireInterval = basefireInterval / attackSpeedStat;

            damageType = DamageType.Shock5s;

            if (base.characterBody.HasBuff(Modules.Buffs.poisonMeleeBuff))
            {
                damageType |= DamageType.BlightOnHit;
            }

            Rimurucon = gameObject.GetComponent<RimuruController>();


            this.animator = base.GetModelAnimator();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            this.animator.SetBool("attacking", true);

            PlayCrossfade("LeftArm, Override", "BlackLightning", "Attack.playbackRate", fireInterval/2, 0.05f);
            this.loopID = AkSoundEngine.PostEvent(186852181, base.gameObject);

            EffectManager.SimpleMuzzleFlash(Modules.Assets.blacklightning, base.gameObject, muzzleString, false);

            this.fireTimer = 0f;

            base.characterBody.SetAimTimer(2f);

            //mainBlacklightning.Stop();
            beamPlay = false;

            attack = new BulletAttack
            {
                bulletCount = 1,
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = damageStat * damageCoefficient,
                damageColorIndex = DamageColorIndex.Default,
                damageType = damageType,
                falloffModel = BulletAttack.FalloffModel.None,
                maxDistance = range,
                force = 0f,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = 0f,
                maxSpread = 0f,
                isCrit = base.RollCrit(),
                owner = base.gameObject,
                muzzleName = muzzleString,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoefficient,
                radius = 1f,
                sniper = false,
                stopperMask = LayerIndex.noCollision.mask,
                weapon = null,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = Modules.Assets.blacklightningimpactEffect,
            };
        }

        public override void Update()
        {
            base.Update();
            updateAimRay();
            if(beamPlay)
            {
                base.characterDirection.forward = aimRay.direction;
                blacklightning.transform.position = FindModelChild(this.muzzleString).transform.position;
                blacklightning.transform.rotation = Quaternion.LookRotation(aimRay.direction);
            }
        }
        public void updateAimRay()
        {
            aimRay = base.GetAimRay();
        }

        public override void OnExit()
        {
            this.animator.SetBool("attacking", false);
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            PlayCrossfade("LeftArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            base.OnExit();
            AkSoundEngine.StopPlayingID(loopID);
            mainBlacklightning.Stop();
            if (NetworkServer.active)
            {
                NetworkServer.Destroy(blacklightning);
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge > fireInterval)
            {
                if (!beamPlay)
                {
                    if (NetworkServer.active)
                    {
                        NetworkServer.Spawn(blacklightning);
                    }
                    mainBlacklightning = blacklightning.GetComponent<ParticleSystem>();
                    mainBlacklightning.Play();
                    beamPlay = true;
                }
                fireTimer += Time.fixedDeltaTime;
                //Fire the laser
                if (fireTimer > fireInterval)
                {
                    PlayCrossfade("LeftArm, Override", "BlackLightningLoop", "Attack.playbackRate", fireInterval, 0.05f);
                    base.characterBody.SetAimTimer(2f);
                    attack.muzzleName = muzzleString;
                    attack.aimVector = aimRay.direction;
                    attack.origin = FindModelChild(this.muzzleString).position;
                    attack.Fire();
                    fireTimer = 0f;
                }

            }
            if (base.fixedAge > totalDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }

        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
