using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


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

                //Attempt transform
                characterMaster.TransformBody(selectedBody);

                //Perform Finalization on all clients.
                new UpdateControllers(netID, health).Send(R2API.Networking.NetworkDestination.Clients);
            }
        }
    }
}
