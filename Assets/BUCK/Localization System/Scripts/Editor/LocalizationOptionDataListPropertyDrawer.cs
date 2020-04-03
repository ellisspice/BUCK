using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BUCK.LocalizationSystem.Editor
{
    [CustomPropertyDrawer(typeof(LocalizationOptionDataList), true)]
    public class LocalizationOptionDataListPropertyDrawer : PropertyDrawer
    {
        protected ReorderableList _reorderableList;

        protected virtual void Init(SerializedProperty property)
        {
            if (_reorderableList != null)
            {
                return;
            }
                
            var array = property.FindPropertyRelative("_options");
            _reorderableList = new ReorderableList(property.serializedObject, array)
            {
                drawElementCallback = DrawOptionData,
                drawHeaderCallback = DrawHeader
            };
            _reorderableList.elementHeight += EditorGUIUtility.singleLineHeight * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);
            _reorderableList.DoList(position);
        }

        protected virtual void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Options");
        }

        protected virtual void DrawOptionData(Rect rect, int index, bool isActive, bool isFocused)
        {
            var itemData = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var itemKey = itemData.FindPropertyRelative("_key");
            var itemImage = itemData.FindPropertyRelative("_image");

            var offset = new RectOffset(0, 0, -1, -3);
            rect = offset.Add(rect);
            rect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(rect, itemKey, GUIContent.none);
            rect.y += EditorGUIUtility.singleLineHeight;
            var validKeys = string.IsNullOrWhiteSpace(itemKey.stringValue) ? LocalizationEditor.Keys : LocalizationEditor.Keys.Where(k => k.ToLower().StartsWith(itemKey.stringValue.ToLower(), StringComparison.Ordinal)).ToList();
            if (validKeys.Count > 0)
            {
                var keyIndex = validKeys.FindIndex(k => string.Equals(k.ToLower(), itemKey.stringValue.ToLower()));
                if (keyIndex < 0)
                {
                    validKeys.Insert(0, string.Empty);
                    keyIndex = 0;
                }
                var value = validKeys[EditorGUI.Popup(rect, string.Empty, keyIndex, validKeys.ToArray())];
                if (!string.IsNullOrEmpty(value))
                {
                    itemKey.stringValue = value;
                }
            }
            else
            {
                EditorGUI.LabelField(rect, "No Matching Keys", EditorStyles.centeredGreyMiniLabel);
            }
            rect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, itemImage, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);
            return _reorderableList.GetHeight();
        }
    }
}