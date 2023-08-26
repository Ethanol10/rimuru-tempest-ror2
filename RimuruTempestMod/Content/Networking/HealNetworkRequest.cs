using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace RimuruMod.Modules.Networking
{
    internal class HealNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        float healthAmount;

        //Don't network these.
        GameObject bodyObj;

        public HealNetworkRequest()
        {

        }

        public HealNetworkRequest(NetworkInstanceId netID, float healthAmount)
        {
            this.netID = netID;
            this.healthAmount = healthAmount;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            healthAmount = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(healthAmount);
        }

        public void OnReceived()
        {
            if (NetworkServer.active)
            {
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                charBody.healthComponent.Heal(healthAmount, new ProcChainMask(), true);
            }
            

        }
    }
}
