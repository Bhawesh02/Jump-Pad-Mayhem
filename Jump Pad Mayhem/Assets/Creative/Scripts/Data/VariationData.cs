using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace KWCreative
{
    [CreateAssetMenu(fileName = "New Variation Data", menuName = "Creative/3DBalls/New Variation Data")]
    public class VariationData : SerializedScriptableObject
    {
        [InlineEditor]
        public GridData gridData;
        [BoxGroup("Variation General", centerLabel: true)]
        public VariationType variationType;
        [BoxGroup("Variation General")]
        public int maxTrajectorySimulationIteration = 50;
        [BoxGroup("Variation General")] 
        public Vector3 environnmetOffset;
        [BoxGroup("Cannon Data", centerLabel: true), InlineEditor]
        public CannonData cannonData;
        [BoxGroup("Cannon Data")] 
        public Vector3 cannonSpawnRotation;
        [BoxGroup("JumpPad Data", centerLabel: true), InlineEditor]
        public JumpPadData jumpPadData;

        [BoxGroup("Level Grid", centerLabel: true)]
        public LevelElementSlotData[,] levelElementsSlots;
        [BoxGroup("Level Grid")]
        public ElementType elementTypeToPutInSlot;
        [BoxGroup("Level Grid")]
        public ElementOrientationType elementRotationToPut;

        [BoxGroup("Level Grid"), Button]
        private void ClearLevel()
        {
            levelElementsSlots = new LevelElementSlotData[gridData.row,gridData.column];
        }

        public void SaveData()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
