using UnityEditor;
using UnityEngine;

namespace BUCK.AudioManagement.Music.Editor
{
    [CustomPropertyDrawer(typeof(MusicTrack), true)]
    public class MusicTrackPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = property.FindPropertyRelative("TrackName").stringValue;
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