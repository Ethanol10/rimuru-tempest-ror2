using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace RimuruMod.Modules.Networking
{
    internal class OrbDamageRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId enemynetID;
        NetworkInstanceId charnetID;
        Vector3 direction;
        private float force;
        private float damage;

        //Don't network these.
        GameObject bodyObj;
        GameObject charbodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = AssetsRimuru.loaderOmniImpactLightningEffect;

        public OrbDamageRequest()
        {

        }

        public OrbDamageRequest(NetworkInstanceId enemynetID, float damage, NetworkInstanceId charnetID)
        {
            this.enemynetID = enemynetID;
            this.damage = damage;
            this.charnetID = charnetID;
        }

        public void Deserialize(NetworkReader reader)
        {
            enemynetID = reader.ReadNetworkId();
            direction = reader.ReadVector3();
            force = reader.ReadSingle();
            damage = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(enemynetID);
            writer.Write(direction);
            writer.Write(force);
            writer.Write(damage);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                search = new BullseyeSearch();
                //Spawn the effect around this object.
                GameObject enemymasterobject = Util.FindNetworkObject(enemynetID);
                CharacterMaster enemycharMaster = enemymasterobject.GetComponent<CharacterMaster>();
                CharacterBody enemycharBody = enemycharMaster.GetBody();
                bodyObj = enemycharBody.gameObject;

                GameObject charmasterobject = Util.FindNetworkObject(charnetID);
                CharacterMaster charcharMaster = charmasterobject.GetComponent<CharacterMaster>();
                CharacterBody charcharBody = charcharMaster.GetBody();
                charbodyObj = charcharBody.gameObject;


                LightningOrb lightningOrb = new LightningOrb();
                lightningOrb.lightningType = LightningOrb.LightningType.Ukulele;
                lightningOrb.damageValue = damage;
                lightningOrb.isCrit = charcharBody.RollCrit();
                lightningOrb.teamIndex = TeamComponent.GetObjectTeam(charcharBody.gameObject);
                lightningOrb.attacker = charbodyObj;
                lightningOrb.procCoefficient = 0f;
                lightningOrb.bouncesRemaining = StaticValues.refractionBounces;
                lightningOrb.speed = 30f;
                lightningOrb.bouncedObjects = new List<HealthComponent>();
                lightningOrb.canBounceOnSameTarget = false;
                lightningOrb.range = StaticValues.refractionRange;
                lightningOrb.damageCoefficientPerBounce = 1f;

                HurtBox hurtBox = enemycharBody.mainHurtBox;
                if (hurtBox)
                {
                    lightningOrb.origin = enemycharBody.corePosition;
                    lightningOrb.target = hurtBox;
                    OrbManager.instance.AddOrb(lightningOrb);
                }
            }
        }


    }
}