using UnityEngine;

namespace RimuruMod.Modules.Survivors
{
    internal class RimuruSwordDisplayController : MonoBehaviour
    {
        public Transform swordTargetTransform;
        public Transform swordTransform;


        public void Update()
        {
            if (swordTargetTransform && swordTargetTransform)
            {
                swordTransform.position = swordTargetTransform.position;
                swordTransform.rotation = swordTargetTransform.rotation;
            }
        }

    }
}