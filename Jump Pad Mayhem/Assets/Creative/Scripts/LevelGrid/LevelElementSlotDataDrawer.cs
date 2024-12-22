using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace KWCreative
{
    internal sealed class LevelElementSlotDataDrawer<TArray> : TwoDimensionalArrayDrawer<TArray, LevelElementSlotData>
        where TArray : IList
    {
        private const string ARROW_TEXTURE_PATH = "Assets/Creative/Arrow/";
        private const string RIGHT_ARROW_TEXTURE_NAME = "Right";
        private const string LEFT_ARROW_TEXTURE_NAME = "Left";
        private const string TOP_ARROW_TEXTURE_NAME = "Top";
        private const string TOP_RIGHT_ARROW_TEXTURE_NAME = "TopRight";
        private const string TOP_LEFT_ARROW_TEXTURE_NAME = "TopLeft";
        private const string BOTTOM_ARROW_TEXTURE_NAME = "Bottom";
        private const string BOTTOM_RIGHT_ARROW_TEXTURE_NAME = "BottomRight";
        private const string BOTTOM_LEFT_ARROW_TEXTURE_NAME = "BottomLeft";
        
        protected override TableMatrixAttribute GetDefaultTableMatrixAttributeSettings()
        {
            return new TableMatrixAttribute()
            {
                SquareCells = true,
                HideColumnIndices = true,
                HideRowIndices = true,
                ResizableColumns = false,
                Transpose = true
            };
        }

        protected override LevelElementSlotData DrawElement(Rect rect, LevelElementSlotData value)
        {
            DrawRectColorAndTexture(rect, value);
            if (CanDrawLevel(rect))
                return value;
            ElementType elementToSwitchInto = CreativeConfig.Instance.CurrentVariationData.elementTypeToPutInSlot;
            ElementOrientationType elementOrientationType = CreativeConfig.Instance.CurrentVariationData.elementRotationToPut;
            if (value.elementType == elementToSwitchInto && value.orientationType == elementOrientationType)
            {
                return value;
            }
            value = new LevelElementSlotData();
            if (elementToSwitchInto == ElementType.CANNON)
            {
                RemoveOldCannon(CreativeConfig.Instance.CurrentVariationData.levelElementsSlots);
            }
            value.elementType = elementToSwitchInto;
            value.orientationType = elementOrientationType;
            GUI.changed = true;
            Event.current.Use();
            EditorUtility.SetDirty(CreativeConfig.Instance.CurrentVariationData);
            AssetDatabase.SaveAssets();
            return value;
        }

        private void DrawRectColorAndTexture(Rect rect, LevelElementSlotData value)
        {
            GetColorAndTextureForSlot(value, out Color color, out Texture texture);
            EditorGUI.DrawRect(rect.Padding(1), color);
            if (value.elementType != ElementType.NULL)
            {
                GUI.color = Color.clear;
                EditorGUI.DrawTextureTransparent(rect.Padding(2), texture, ScaleMode.ScaleToFit);
                GUI.color = Color.white;
            }
        }

        private void GetColorAndTextureForSlot(LevelElementSlotData value, out Color color, out Texture texture)
        {
            color = value.elementType switch
            {
                ElementType.NULL => Color.clear,
                ElementType.CANNON => Color.blue,
                ElementType.JUMPPAD => Color.green,
                _ => throw new ArgumentOutOfRangeException()
            };
            string textureString = value.orientationType switch
            {
                ElementOrientationType.TOP => TOP_ARROW_TEXTURE_NAME,
                ElementOrientationType.BOTTOM => BOTTOM_ARROW_TEXTURE_NAME,
                ElementOrientationType.LEFT => LEFT_ARROW_TEXTURE_NAME,
                ElementOrientationType.RIGHT => RIGHT_ARROW_TEXTURE_NAME,
                ElementOrientationType.TOP_LEFT => TOP_LEFT_ARROW_TEXTURE_NAME,
                ElementOrientationType.TOP_RIGHT => TOP_RIGHT_ARROW_TEXTURE_NAME,
                ElementOrientationType.BOTTOM_LEFT => BOTTOM_LEFT_ARROW_TEXTURE_NAME,
                ElementOrientationType.BOTTOM_RIGHT => BOTTOM_RIGHT_ARROW_TEXTURE_NAME,
                _ => throw new ArgumentOutOfRangeException()
            };
            texture = AssetDatabase.LoadAssetAtPath<Texture>(ARROW_TEXTURE_PATH + textureString + ".png");;
        }

        private  bool CanDrawLevel(Rect rect)
        {
            return Event.current.type != EventType.MouseDrag ||
                   Event.current.button != 0 || !rect.Contains(Event.current.mousePosition);
        }
        
        private void RemoveOldCannon(LevelElementSlotData[,] levelElementSlots)
        {
            for (int rowIndex = 0; rowIndex < levelElementSlots.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < levelElementSlots.GetLength(1); columnIndex++)
                {
                    LevelElementSlotData levelElementsSlotData = levelElementSlots[rowIndex, columnIndex];
                    if (levelElementsSlotData.elementType == ElementType.CANNON)
                    {
                        levelElementSlots[rowIndex, columnIndex] = new();
                    }
                }
            }
        }
        
    }
}