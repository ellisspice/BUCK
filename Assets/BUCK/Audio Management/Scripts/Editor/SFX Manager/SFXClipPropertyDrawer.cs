using UnityEditor;
using UnityEngine;

namespace BUCK.AudioManagement.SFX.Editor
{
    [CustomPropertyDrawer(typeof(SFXClip), true)]
    public class SFXClipPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = property.FindPropertyRelative("Name").stringValue;
            if (string.IsNullOrWhiteSpace(name))
            {
                var clip = property.FindPropertyRelative("Clip").objectReferenceValue;
                if (clip)
                {
                    name = clip.name;
                }
            }
            label.text = string.IsNullOrWhiteSpace(name) ? label.text : name;
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}