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
        private float radius = 10f;
        private float range = 60f;
        private float damageCoefficient = Modules.StaticValues.blacklightningDamageCoefficient;
        private float procCoefficient = Modules.StaticValues.blacklightningProcCoefficient;
        private float force = 100f;
        private float fireTimer;
        public string muzzleString = "RWrist";
        //public LoopSoundDef loopSoundDef = Modules.Assets.xiconstructsound;
        //private LoopSoundManager.SoundLoopPtr loopPtr;

        private GameObject blacklightning;
        private ParticleSystem mainBlacklightning;
        private BulletAttack attack;
        private BlastAttack blastAttack;
        private float fireInterval = 0.1f;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            updateAimRay();
            base.characterBody.SetAimTimer(2f);
            damageType = DamageType.Shock5s;
            Rimurucon = gameObject.GetComponent<RimuruController>();


            this.animator = base.GetModelAnimator();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            //PlayCrossfade("RightArm, Override", "RightArmOut", "Attack.playbackRate", duration, 0.1f);
            //AkSoundEngine.PostEvent(3660048432, base.gameObject);

            //EffectManager.SimpleMuzzleFlash(Modules.Assets.blacklightning, base.gameObject, muzzleString, false);
            //if (this.loopSoundDef)
            //{
            //    this.loopPtr = LoopSoundManager.PlaySoundLoopLocal(base.gameObject, this.loopSoundDef);
            //}
            this.fireTimer = 0f;

            base.characterBody.SetAimTimer(2f);

            blacklightning = UnityEngine.Object.Instantiate(Modules.Assets.blacklightning);
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(blacklightning);
            }
            mainBlacklightning = blacklightning.GetComponent<ParticleSystem>();
            mainBlacklightning.Stop();
            beamPlay = false;

            attack = new BulletAttack
            {
                bulletCount = 1,
                aimVector = aimRay.direction,
                origin = FindModelChild(this.muzzleString).transform.position,
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
                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                //hitCallback = laserHitCallback
            };
        }

        //public bool laserHitCallback(BulletAttack bulletRef, ref BulletHit hitInfo)
        //{
        //    var hurtbox = hitInfo.hitHurtBox;
        //    if (hurtbox)
        //    {
        //        var healthComponent = hurtbox.healthComponent;
        //        if (healthComponent)
        //        {
        //            var body = healthComponent.body;
        //            if (body)
        //            {
        //                new BlastAttack
        //                {
        //                    attacker = base.gameObject,
        //                    teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
        //                    falloffModel = BlastAttack.FalloffModel.None,
        //                    baseDamage = damageStat * Modules.StaticValues.blacklightningDamageCoefficient,
        //                    damageType = DamageType.Shock5s,
        //                    damageColorIndex = DamageColorIndex.Default,
        //                    baseForce = 0,
        //                    position = body.transform.position,
        //                    radius = 1f,
        //                    procCoefficient = 1f,
        //                    attackerFiltering = AttackerFiltering.NeverHitSelf,
        //                }.Fire();

        //                if (body.HasBuff(Modules.Buffs.WetLightningDebuff))
        //                {
        //                    body.RemoveBuff(Modules.Buffs.WetLightningDebuff);

        //                }
                        
        //            }
        //        }
        //    }
        //    return false;
        //}
        public override void Update()
        {
            base.Update();
            updateAimRay();
            base.characterDirection.forward = aimRay.direction;
            blacklightning.transform.position = FindModelChild(this.muzzleString).transform.position;
            blacklightning.transform.rotation = Quaternion.LookRotation(aimRay.direction);
        }
        public void updateAimRay()
        {
            aimRay = base.GetAimRay();
        }

        public override void OnExit()
        {
            this.animator.SetBool("attacking", false);
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            //PlayCrossfade("LeftArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            base.OnExit();
            //LoopSoundManager.StopSoundLoopLocal(this.loopPtr);
            mainBlacklightning.Stop();
            if (NetworkServer.active)
            {
                NetworkServer.Destroy(blacklightning);
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.IsKeyDownAuthority())
            {
                if (!beamPlay)
                {
                    mainBlacklightning.Play();
                    beamPlay = true;
                }
                fireTimer += Time.fixedDeltaTime;
                //Fire the laser
                if (fireTimer > fireInterval)
                {
                    //PlayCrossfade("LeftArm, Override", "LeftArmOut", "Attack.playbackRate", fireInterval, 0.1f);
                    base.characterBody.SetAimTimer(2f);
                    attack.muzzleName = muzzleString;
                    attack.aimVector = aimRay.direction;
                    attack.origin = FindModelChild(this.muzzleString).position;
                    attack.Fire();
                    fireTimer = 0f;
                }

            }
            else
            {
                base.outer.SetNextStateToMain();
            }

        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
