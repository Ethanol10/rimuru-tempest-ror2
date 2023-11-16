using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using EntityStates.ScavMonster;
using RimuruMod.Modules;

namespace RimuruMod.Modules.Networking
{
    public class ItemDropNetworked : INetMessage
    {
        //network
        NetworkInstanceId netID;

        //don't network
        GameObject bodyObj;
        PickupIndex dropPickup;
        int itemsToGrant;

        public ItemDropNetworked()
        {

        }

        public ItemDropNetworked(NetworkInstanceId netID)
        {
            this.netID = netID;
        }


        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
        }
        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
        }

        public void OnReceived()
        {
            if (NetworkServer.active)
            {
                
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                Util.PlaySound(FindItem.sound, bodyObj);
                WeightedSelection<List<PickupIndex>> weightedSelection = new WeightedSelection<List<PickupIndex>>(8);
                weightedSelection.AddChoice(Run.instance.availableTier1DropList.Where(new Func<PickupIndex, bool>(this.PickupIsNonBlacklistedItem)).ToList<PickupIndex>(), FindItem.tier1Chance);
                weightedSelection.AddChoice(Run.instance.availableTier2DropList.Where(new Func<PickupIndex, bool>(this.PickupIsNonBlacklistedItem)).ToList<PickupIndex>(), FindItem.tier2Chance);
                weightedSelection.AddChoice(Run.instance.availableTier3DropList.Where(new Func<PickupIndex, bool>(this.PickupIsNonBlacklistedItem)).ToList<PickupIndex>(), FindItem.tier3Chance);
                List<PickupIndex> list = weightedSelection.Evaluate(UnityEngine.Random.value);
                dropPickup = list[UnityEngine.Random.Range(0, list.Count)];
                PickupDef pickupDef = PickupCatalog.GetPickupDef(this.dropPickup);
                if (pickupDef != null)
                {
                    ItemDef itemDef = ItemCatalog.GetItemDef(pickupDef.itemIndex);
                    if (itemDef != null)
                    {
                        this.itemsToGrant = 0;
                        switch (itemDef.tier)
                        {
                            case ItemTier.Tier1:
                                this.itemsToGrant = StaticValues.tier1Amount;
                                break;
                            case ItemTier.Tier2:
                                this.itemsToGrant = StaticValues.tier2Amount;
                                break;
                            case ItemTier.Tier3:
                                this.itemsToGrant = StaticValues.tier3Amount;
                                break;
                            default:
                                this.itemsToGrant = 1;
                                break;
                        }
                    }
                }

                GrantPickupServer(dropPickup, charBody, itemsToGrant);
            }
        }

        private bool PickupIsNonBlacklistedItem(PickupIndex pickupIndex)
        {
            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
            if (pickupDef == null)
            {
                return false;
            }
            ItemDef itemDef = ItemCatalog.GetItemDef(pickupDef.itemIndex);
            return !(itemDef == null);
        }

        private void GrantPickupServer(PickupIndex pickupIndex, CharacterBody charBody, int itemsToGrant)
        {
            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
            {
                if (pickupDef == null)
                {
                    return;
                }
                ItemIndex itemIndex = pickupDef.itemIndex;
                if (ItemCatalog.GetItemDef(itemIndex) == null)
                {
                    return;
                }
                charBody.inventory.GiveItem(itemIndex, itemsToGrant);
            }
        }
    }
}
