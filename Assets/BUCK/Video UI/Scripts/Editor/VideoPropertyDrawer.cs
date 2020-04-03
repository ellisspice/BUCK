using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace BUCK.VideoUI.Editor
{
    [CustomPropertyDrawer(typeof(Video))]
    public class VideoPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
 
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var sourceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var selectRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
            var source = property.FindPropertyRelative("VideoSource");
            var clip = property.FindPropertyRelative("VideoClip");
            var url = property.FindPropertyRelative("VideoURL");
            source.intValue = EditorGUI.Popup(sourceRect, "Video Source", source.intValue, source.enumNames);
            switch ((VideoSource)source.intValue)
            {
                case VideoSource.VideoClip:
                    clip.objectReferenceValue = EditorGUI.ObjectField(selectRect, "Video Clip", clip.objectReferenceValue, typeof(VideoClip), false);
                    break;
                case VideoSource.Url:
                    url.stringValue = EditorGUI.TextField(selectRect, "Video URL", url.stringValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}