using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore;


namespace RimuruMod.Modules.Networking
{
    internal class TransformBody : INetMessage
    {
        public enum TargetBody : int
        {
            HUMAN = 0,
            SLIME = 1,
        }

        //Network these ones.
        NetworkInstanceId netID;
        float health;
        TargetBody targetBody;


        //Don't network these.
        GameObject bodyObj;

        public TransformBody()
        {

        }

        public TransformBody(NetworkInstanceId netID, float health, int targetBody)
        {
            this.netID = netID;
            this.health = health;
            this.targetBody = (TargetBody)targetBody;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            health = reader.ReadSingle();
            targetBody = (TargetBody)reader.ReadInt32();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(health);
            writer.Write((int)targetBody);
        }

        public void OnReceived()
        {
            if (NetworkServer.active) 
            {
                GameObject charmasterobject = Util.FindNetworkObject(netID);
                CharacterMaster characterMaster = charmasterobject.GetComponent<CharacterMaster>();


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

                StupidExternalSpawningBodyFunction(characterMaster, newBody);

                //Perform Finalization on all clients.
                new UpdateControllers(netID, health).Send(R2API.Networking.NetworkDestination.Clients);
            }
        }

        public CharacterBody StupidExternalSpawningBodyFunction(CharacterMaster master, GameObject bodyPrefab) 
        {
            Vector3 footPosition = master.GetBody().transform.position;
            Quaternion rotation = master.GetBody().transform.rotation;

            //Destroy body first.
            master.DestroyBody();

            //Do spawning thing. This was ripped from Respawn(), but we didn't want to use the metamorphisis stuff so this is why it's a clone.
            if (bodyPrefab)
            {
                CharacterBody component = bodyPrefab.GetComponent<CharacterBody>();
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
                    bodyPrefab
                });
            }
            else 
            {
                Debug.Log("LMAO failed to spawn.");
            }

            return null;
        }
    }
}
