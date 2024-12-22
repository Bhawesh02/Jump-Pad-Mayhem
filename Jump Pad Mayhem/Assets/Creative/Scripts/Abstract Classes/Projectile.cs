using UnityEngine;

namespace KWCreative
{
    public abstract class Projectile : GhostObject
    {
        [SerializeField] protected Rigidbody m_rigidbody;
        
        protected Cannon m_cannon;

        public void SetCannon(Cannon cannon)
        {
            m_cannon = cannon;
        }

        public abstract void SetVelocity(Vector3 velocityVector);
    }
}

