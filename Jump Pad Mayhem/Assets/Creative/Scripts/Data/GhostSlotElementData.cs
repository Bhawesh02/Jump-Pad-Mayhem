using UnityEngine;

[CreateAssetMenu(fileName = "GhostSlotElementData", menuName = "Grid/GhostSlotElementData", order = 1)]
public class GhostSlotElementData : ScriptableObject
{
    public Vector3 positionOnSlot;
    public GhostSlotElement prefab;
}