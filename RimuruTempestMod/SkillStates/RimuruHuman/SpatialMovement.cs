using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class SpatialMovement : BaseSkillState
    {
        private bool teleportFlag = false;
        private float teleportTimer = 0f;
        private float teleportDuration = 0.1f;
        private Vector3 velocity;
        private Vector3 teleportLocation = new Vector3(0, 0, 0);
        private bool exitState = false;

        private GameObject aimSphere;
        public float radius = 3f;
        private Ray aimRay;
        private float maxDistance = 75f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.aimSphere = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            AkSoundEngine.PostEvent("RimuruBlink", base.gameObject);
        }
        public override void Update()
        {
            base.Update();
            this.UpdateAreaIndicator();
        }
        private void UpdateAreaIndicator()
        {
            bool isAuthority = base.isAuthority;
            bool flag = isAuthority;
            if (flag)
            {
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);
            }
            this.aimRay = base.GetAimRay();
            RaycastHit raycastHit;
            bool flag2 = Physics.Raycast(base.GetAimRay(), out raycastHit, this.maxDistance, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
            bool flag3 = flag2;
            if (flag3)
            {
                this.aimSphere.transform.position = raycastHit.point + Vector3.up;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = -this.aimRay.direction;
            }
            else
            {
                Ray ray = base.GetAimRay();
                Vector3 position = ray.origin + this.maxDistance * ray.direction;
                this.aimSphere.transform.position = position;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = -this.aimRay.direction;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && !base.IsKeyDownAuthority())
            {
                teleportFlag = true;
            }

            if (teleportFlag ) 
            {
                Teleport();
            }

            if (exitState) 
            {
                this.outer.SetNextStateToMain();
            }
        }

        public void Teleport() 
        {
            //Set velocity to 0 throughout teleport duration
            base.characterMotor.velocity.y = 0f;
            teleportTimer += Time.fixedDeltaTime;

            base.characterMotor.Motor.SetPosition(Vector3.SmoothDamp(base.characterBody.corePosition, aimSphere.transform.position, ref velocity, teleportDuration, 1000f));

            if (teleportTimer >= teleportDuration) 
            {
                exitState = true;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            EntityState.Destroy(this.aimSphere.gameObject);
            characterBody.AddTimedBuffAuthority(Modules.Buffs.SpatialMovementBuff.buffIndex, Modules.Config.spatialMovementBuffDuration.Value);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}