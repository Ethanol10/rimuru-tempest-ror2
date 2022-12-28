using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RimuruMod.SkillStates
{
    public class SpatialMovement : BaseSkillState
    {
        private GameObject aimSphere;
        public float radius = 3f;
        private Ray aimRay;
        private float maxDistance = 150f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.aimSphere = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            AkSoundEngine.PostEvent(3379926649, base.gameObject);
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
                base.characterMotor.rootMotion += this.aimSphere.transform.position - base.characterBody.corePosition;
                this.outer.SetNextStateToMain();
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