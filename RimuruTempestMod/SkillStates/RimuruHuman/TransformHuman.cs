using EntityStates;
using R2API.Networking.Interfaces;
using RimuruMod.Content.BuffControllers;
using RimuruMod.Modules.Networking;
using RimuruMod.Modules.Survivors;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static RimuruMod.Modules.Networking.TransformBody;

namespace RimuruMod.SkillStates
{
    public class TransformHuman : BaseSkillState
    {
        public float oldHealth;
        public RimuruMasterController masterController;

        public override void OnEnter()
        {
            base.OnEnter();
            masterController = characterBody.master.GetComponent<RimuruMasterController>();
            oldHealth = characterBody.healthComponent.health;
            //new TransformBody(characterBody.master.netId, oldHealth, (int)TransformBody.TargetBody.SLIME).Send(R2API.Networking.NetworkDestination.Clients);
            if (NetworkServer.active) 
            {
                this.TransformBody(RimuruMod.Modules.Networking.TransformBody.TargetBody.SLIME);
            }
            AkSoundEngine.PostEvent("RimuruTransform", base.gameObject);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > 0f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        internal void TransformBody(TransformBody.TargetBody targetBody) 
        {
            CharacterMaster characterMaster = base.characterBody.master;

            string selectedBody = "";
            switch (targetBody)
            {
                case TargetBody.HUMAN:
                    selectedBody = "RimuruHumanBody";
                    break;
                case TargetBody.SLIME:
                    selectedBody = "RimuruSlimeBody";
                    break;
                default:
                    selectedBody = "RimuruHumanBody";
                    break;
            }

            GameObject newBody = BodyCatalog.FindBodyPrefab(selectedBody);


            //Attempt transform
            //characterMaster.TransformBody(selectedBody);

            //Set master body prefab before calling this func.
            characterMaster.bodyPrefab = newBody;
            StupidExternalSpawningBodyFunction(characterMaster);

            ////Perform Finalization on all clients.
            //new UpdateControllers(netID, health).Send(R2API.Networking.NetworkDestination.Clients);
        }

        public override void OnExit()
        {
            base.OnExit();

            //RimuruBaseBuffController[] array = characterBody.master.GetComponents<RimuruBaseBuffController>();
            //foreach (RimuruBaseBuffController controller in array)
            //{
            //    controller.UpdateBody();
            //}
            //characterBody.master.GetBody().healthComponent.health = oldHealth;
            new UpdateControllers(characterBody.master.netId, oldHealth).Send(R2API.Networking.NetworkDestination.Clients);
            masterController.setHealthToValue = true;
            masterController.oldHealth = oldHealth;
        }

        public CharacterBody StupidExternalSpawningBodyFunction(CharacterMaster master)
        {
            Vector3 footPosition = master.GetBody().transform.position;
            Quaternion rotation = master.GetBody().transform.rotation;

            //Destroy body first.
            master.DestroyBody();

            //Do spawning thing. This was ripped from Respawn(), but we didn't want to use the metamorphisis stuff so this is why it's a clone.
            if (master.bodyPrefab)
            {
                CharacterBody component = master.bodyPrefab.GetComponent<CharacterBody>();
                if (component)
                {
                    Vector3 position = footPosition;
                    if (true)
                    {
                        position = master.CalculateSafeGroundPosition(footPosition, component);
                    }
                    return master.SpawnBody(position, rotation);
                }
                Debug.LogErrorFormat("Trying to respawn as object {0} who has no Character Body!", new object[]
                {
                    master.bodyPrefab
                });
            }
            else
            {
                Debug.Log("LMAO failed to spawn.");
            }

            return null;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}