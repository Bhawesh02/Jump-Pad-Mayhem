using Sirenix.OdinInspector;
using UnityEngine;

namespace KWCreative
{
    [CreateAssetMenu(fileName = "New Cannon Data", menuName = "Creative/3DBalls/New Cannon Data")]

    public class CannonData : GhostSlotElementData
    {
        [TabGroup("Data", "Cannon Config"), Min(0f)]
        public float maxCannonRotationAngel = 45f;
        [TabGroup("Data", "Cannon Config"), Min(0f)]
        public float cannonRotationSpeed = 4f;
        [TabGroup("Data", "Projectile Config"), Min(0f)]
        public int projectileCount = 1;
        [TabGroup("Data", "Projectile Config"), Min(0f)]
        public float projectileFireForce = 57.29f;
        [TabGroup("Data", "Projectile Config"), Min(0f)]
        public float projectileFireDelay = 0.25f;
    }
}

