using Sirenix.OdinInspector;
using UnityEngine;

namespace KWCreative
{
    public abstract class GhostObject : SerializedMonoBehaviour
    {
        [BoxGroup("Ghost Object Reference", centerLabel:true)]
        [SerializeField] protected Renderer m_objectRenderer;
        [BoxGroup("Ghost Object Reference")]
        [SerializeField] protected GameObject m_modelObject;
        
        protected bool m_isGhost;
        protected Transform m_ghostObjectTransform;
        
        public virtual void SetGhostObjectTransform(Transform ghostTransform) =>
            m_ghostObjectTransform = ghostTransform;
        
        public virtual void SetIsGhost()
        {
            m_isGhost = true;
            if (m_objectRenderer)
            {
                m_objectRenderer.enabled = false;
            }
            if (m_modelObject)
            {
                m_modelObject.SetActive(false);
            }
        }
    }
}
