using R2API.Networking.Interfaces;
using RimuruMod.Content.BuffControllers;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace RimuruMod.Modules.Networking
{
    internal class UpdateControllers : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        float health;

        public UpdateControllers()
        {

        }

        public UpdateControllers(NetworkInstanceId netID, float health)
        {
            this.netID = netID;
            this.health = health;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            health = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(health);
        }

        public void OnReceived()
        {
            GameObject charmasterobject = Util.FindNetworkObject(netID);
            if (charmasterobject) 
            {
                CharacterMaster charMaster = charmasterobject.GetComponent<CharacterMaster>();
                if (charMaster) 
                {
                    RimuruBaseBuffController[] array = charmasterobject.GetComponents<RimuruBaseBuffController>();
                    foreach (RimuruBaseBuffController controller in array)
                    {
                        controller.UpdateBody();
                    }

                    if (NetworkServer.active) 
                    {
                        charMaster.GetBody().healthComponent.Networkhealth = health;
                    }   
                }
            }
        }
    }
}
