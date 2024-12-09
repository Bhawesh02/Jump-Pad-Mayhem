using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KWCreative
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private LooperGrid m_grid;
        [SerializeField] private GameObject m_environment;

        private VariationData m_currentVariationData;
        private List<GhostSlotElement> m_spawnedElements;
        private List<JumpSurface> m_jumpSurfaces = new ();

        private Dictionary<ElementOrientationType, Vector3> RotationDataBasedOnAxis = new()
        {
            {ElementOrientationType.TOP, new Vector3(0,180f,0f)},
            {ElementOrientationType.BOTTOM, new Vector3(180f, 0f, 0f)},
            {ElementOrientationType.LEFT, new Vector3(0, -180f, -90f)},
            {ElementOrientationType.RIGHT, new Vector3(0, -180f, 90f)},
            {ElementOrientationType.TOP_LEFT, new Vector3(0f, 180f, -45f)},
            {ElementOrientationType.TOP_RIGHT, new Vector3(0f, 180f, 45f)},
            {ElementOrientationType.BOTTOM_LEFT, new Vector3(-180f, 0f, 45f)},
            {ElementOrientationType.BOTTOM_RIGHT, new Vector3(-180f, 0f, -45f)}
        };

        public List<JumpSurface> JumpSurfaces => m_jumpSurfaces;
        
        private void Awake()
        {
            GenerateLevel();
        }

        [ButtonGroup, Button("Clear Level", ButtonSizes.Medium)]
        private void ClearLevel()
        {
            if (m_grid.Slots != null)
            {
                m_grid.ClearGrid();
            }
            if (m_spawnedElements != null)
            {
                m_spawnedElements.Clear();
            }
            else
            {
                m_spawnedElements = new ();
            }
        } 
            
        
        [ButtonGroup, Button("Generate Level", ButtonSizes.Medium)]
        private void GenerateLevel()
        {
            InitGrid();
            SpawnLevelElements();
            m_environment.transform.position = m_currentVariationData.environnmetOffset;
        }

        private void InitGrid()
        {
            ClearLevel();
            m_currentVariationData = CreativeConfig.Instance.CurrentVariationData;
            m_grid.SetGridData(m_currentVariationData.gridData);
            m_grid.Initialize();
        }

        private void SpawnLevelElements()
        {
            LevelElementSlotData[,] levelElementSlots = m_currentVariationData.levelElementsSlots;
            GameGridItemSlot itemSlotToSpawnElementIn;
            GhostSlotElementData elementData;
           
            for (int rowIndex = 0; rowIndex < levelElementSlots.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < levelElementSlots.GetLength(1); columnIndex++)
                {
                    LevelElementSlotData levelElementsSlotData = levelElementSlots[rowIndex, columnIndex];
                    switch (levelElementsSlotData.elementType)
                    {
                        case ElementType.NULL:
                            continue;
                        case ElementType.CANNON:
                            elementData = m_currentVariationData.cannonData;
                            break;
                        case ElementType.JUMPPAD:
                            elementData = m_currentVariationData.jumpPadData;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    itemSlotToSpawnElementIn = (GameGridItemSlot) m_grid.GetSlot(rowIndex, columnIndex);
                    SpawnElementAtSlot(itemSlotToSpawnElementIn, elementData, levelElementsSlotData);
                }
            }
        }

        private void SpawnElementAtSlot(GameGridItemSlot itemSlotToSpawnElementIn, GhostSlotElementData elementData, LevelElementSlotData levelElementSlotData)
        {
            GhostSlotElement slotElement = Instantiate(elementData.prefab);
            slotElement.Configure(itemSlotToSpawnElementIn,elementData,m_grid);
            itemSlotToSpawnElementIn.OccupySlot(elementData, slotElement);
            SetElementRotationAndPosition(levelElementSlotData, slotElement.transform);
            m_spawnedElements.Add(slotElement);
            if (levelElementSlotData.elementType == ElementType.JUMPPAD)
            {
                m_jumpSurfaces.Add((JumpSurface)slotElement);
            }
        }

        private void SetElementRotationAndPosition(LevelElementSlotData levelElementSlotData, Transform slotElementTransform)
        {
            
            Vector3 rotationToPut;
            if (levelElementSlotData.elementRotation != Vector3.zero)
            {
                rotationToPut = levelElementSlotData.elementRotation;
            }
            else
            {
                RotationDataBasedOnAxis.TryGetValue(levelElementSlotData.orientationType, out rotationToPut);
            }
            if (levelElementSlotData.elementType == ElementType.CANNON)
            {
                rotationToPut = m_currentVariationData.cannonSpawnRotation;
            }
            slotElementTransform.localEulerAngles = rotationToPut;
            slotElementTransform.localPosition = levelElementSlotData.elementPositionOffset;
        }
        
        [Button("Save All element Rotation", ButtonSizes.Medium)]
        private void SaveAllElementRotation()
        {
            foreach (GhostSlotElement spawnedElement in m_spawnedElements)
            {
                m_currentVariationData.levelElementsSlots[spawnedElement.BaseItemSlot.Coord.x, spawnedElement.BaseItemSlot.Coord.y]
                    .elementRotation = spawnedElement.transform.localEulerAngles;
                m_currentVariationData.SaveData();
            }
        }
        [Button("Save All element Position", ButtonSizes.Medium)]
        private void SaveAllElementPosition()
        {
            foreach (GhostSlotElement spawnedElement in m_spawnedElements)
            {
                m_currentVariationData.levelElementsSlots[spawnedElement.BaseItemSlot.Coord.x, spawnedElement.BaseItemSlot.Coord.y]
                    .elementPositionOffset = spawnedElement.transform.localPosition;
                m_currentVariationData.SaveData();
            }
        }
    }
    
    
}

