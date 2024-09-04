using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using RimuruMod.Modules;


namespace RimuruMod.Modules.Networking
{
    internal class PeformDirectionalForceNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        Vector3 direction;
        private float force;
        private float damage;
        private float range;

        //Don't network these.
        GameObject bodyObj;
        GameObject dekubodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = AssetsRimuru.voidjailerEffect;

        public PeformDirectionalForceNetworkRequest()
        {

        }

        public PeformDirectionalForceNetworkRequest(NetworkInstanceId netID, Vector3 direction, float force, float damage, float range)
        {
            this.netID = netID;
            this.direction = direction;
            this.force = force;
            this.damage = damage;
            this.range = range;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            direction = reader.ReadVector3();
            force = reader.ReadSingle();
            damage = reader.ReadSingle();
            range = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(direction);
            writer.Write(force);
            writer.Write(damage);
            writer.Write(range);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                search = new BullseyeSearch();
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                //search for target
                SearchForTarget(charBody);
                //Damage target and stun
                DamageTargets(charBody);
            }
        }

        private void DamageTargets(CharacterBody charBody)
        {

            if (trackingTargets.Count > 0)
            {
                foreach (HurtBox singularTarget in trackingTargets)
                {

                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        float Weight = 1f;
                        if (singularTarget.healthComponent.body.characterMotor)
                        {
                            Weight = singularTarget.healthComponent.body.characterMotor.mass;
                        }
                        else if (singularTarget.healthComponent.body.rigidbody)
                        {
                            Weight = singularTarget.healthComponent.body.rigidbody.mass;
                        }



                        DamageInfo damageInfo = new DamageInfo
                        {
                            attacker = dekubodyObj,
                            inflictor = dekubodyObj,
                            damage = damage,
                            position = singularTarget.transform.position,
                            procCoefficient = 1f,
                            damageType = DamageType.Generic,
                            crit = charBody.RollCrit(),

                        };

                        singularTarget.healthComponent.TakeDamageForce(direction * force * (Weight), true, true);
                        singularTarget.healthComponent.TakeDamage(damageInfo);
                        GlobalEventManager.instance.OnHitEnemy(damageInfo, singularTarget.healthComponent.gameObject);


                        EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                        {
                            origin = singularTarget.transform.position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(direction),

                        }, true);

                    }
                }
            }
        }

        private void SearchForTarget(CharacterBody charBody)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(charBody.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = charBody.transform.position;
            this.search.searchDirection = Vector3.up;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = range;
            this.search.maxAngleFilter = 360f;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(charBody.gameObject);
            this.trackingTargets = this.search.GetResults().ToList<HurtBox>();
        }

    }
}
