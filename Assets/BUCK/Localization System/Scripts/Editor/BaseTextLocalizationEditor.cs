using System;
using System.Linq;
using UnityEditor;

namespace BUCK.LocalizationSystem.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(BaseTextLocalization), true)]
    public abstract class BaseTextLocalizationEditor : UILocalizationEditor
    {
        protected SerializedProperty _key;
        protected SerializedProperty _toUpper;
        protected SerializedProperty _formatArgs;
        
        protected virtual void OnEnable()
        {
            _key = serializedObject.FindProperty("_key");
            _toUpper = serializedObject.FindProperty("_toUpper");
            _formatArgs = serializedObject.FindProperty("_formatArgs");
        }
        
        public override void OnInspectorGUI()
        {
            LocalizationEditor.GetKeys();
            serializedObject.Update();
            _key.stringValue = EditorGUILayout.TextField("Key", _key.stringValue);
            var validKeys = string.IsNullOrWhiteSpace(_key.stringValue) ? LocalizationEditor.Keys : LocalizationEditor.Keys.Where(k => k.ToLower().StartsWith(_key.stringValue.ToLower(), StringComparison.Ordinal)).ToList();
            if (validKeys.Count > 0)
            {
                var index = validKeys.FindIndex(k => string.Equals(k.ToLower(), _key.stringValue.ToLower()));
                if (index < 0)
                {
                    validKeys.Insert(0, string.Empty);
                    index = 0;
                }
                var value = validKeys[EditorGUILayout.Popup(string.Empty, index, validKeys.ToArray())];
                if (!string.IsNullOrEmpty(value))
                {
                    _key.stringValue = value;
                }
            }
            else
            {
                EditorGUILayout.LabelField("No Matching Keys", EditorStyles.centeredGreyMiniLabel);
            }
            EditorGUILayout.PropertyField(_toUpper);
            EditorGUILayout.PropertyField(_formatArgs, true);
            serializedObject.ApplyModifiedProperties();
            DrawTestingGUI();
        }
    }
}