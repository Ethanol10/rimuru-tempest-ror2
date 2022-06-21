using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using On.RoR2;
using RoR2;
using static RoR2.Loadout;

namespace RimuruMod.Modules.Survivors
{
    internal class RimuruSwordDisplayController : MonoBehaviour
    {
        public Transform swordTargetTransform;
        public Transform swordTransform;


        public void Update()
        {
            swordTransform = swordTargetTransform;
        }

    }
}