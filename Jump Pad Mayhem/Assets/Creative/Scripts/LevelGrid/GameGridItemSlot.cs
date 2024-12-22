
namespace KWCreative
{
    public class GameGridItemSlot : ItemSlot
    {
        protected GhostSlotElement m_ghostElement;
        protected GhostSlotElementData m_ghostSlotElementData;
        
        public GhostSlotElement GhostSlotElement => m_ghostElement;
        public GhostSlotElementData GhostSlotElementData => m_ghostSlotElementData;
        public virtual void OccupySlot(GhostSlotElementData data, GhostSlotElement element)
        {
            m_ghostSlotElementData = data;
            m_ghostElement = element;
        }
    }
}