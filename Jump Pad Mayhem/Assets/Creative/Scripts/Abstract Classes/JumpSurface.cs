using Sirenix.OdinInspector;

namespace KWCreative
{
    public abstract class JumpSurface : GhostSlotElement
    {

        [Button("Send Transform to Ghost Object")]
        protected virtual void SendTransformToGhostObject()
        {
            if (!m_ghostObjectTransform)
            {
                return;
            }
            m_ghostObjectTransform.position = transform.position;
            m_ghostObjectTransform.rotation = transform.rotation;
        }
        protected abstract void HandelBallCollision(Ball collidedBall);
    }
}
