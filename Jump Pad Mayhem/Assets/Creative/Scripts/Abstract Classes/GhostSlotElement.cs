using System.Collections.Generic;
using KWCreative;

public abstract class GhostSlotElement : GhostObject
{
    protected GhostSlotElementData m_slotElementData;
    protected ItemSlot m_baseItemSlot;

    public ItemSlot BaseItemSlot => m_baseItemSlot;
    
    public virtual void Configure(Slot slot, GhostSlotElementData data, Grid grid)
    {
        m_slotElementData = data;
        m_baseItemSlot = (ItemSlot)slot;
        transform.parent = slot.transform;
        transform.localPosition = data.positionOnSlot;
    }

}