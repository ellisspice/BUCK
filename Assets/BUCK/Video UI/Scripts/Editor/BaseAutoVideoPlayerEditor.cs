using UnityEditor;

namespace BUCK.VideoUI.Editor
{ 
    [CanEditMultipleObjects, CustomEditor(typeof(BaseAutoVideoPlayer<>))]
    public class BaseAutoVideoPlayerEditor : UnityEditor.Editor 
    {
        protected SerializedProperty _video;
        protected SerializedProperty _waitForFirstFrame;
        protected SerializedProperty _skipOnDrop;
        protected SerializedProperty _playbackSpeed;
        protected SerializedProperty _volume;
        protected SerializedProperty _mute;
        protected SerializedProperty _demoInactivityTime;
        protected SerializedProperty _timeToFade;

        protected virtual void OnEnable()
        {
            _video = serializedObject.FindProperty("_video");
            _waitForFirstFrame = serializedObject.FindProperty("_waitForFirstFrame");
            _skipOnDrop = serializedObject.FindProperty("_skipOnDrop");
            _playbackSpeed = serializedObject.FindProperty("_playbackSpeed");
            _volume = serializedObject.FindProperty("_volume");
            _mute = serializedObject.FindProperty("_mute");
            _demoInactivityTime = serializedObject.FindProperty("_demoInactivityTime");
            _timeToFade = serializedObject.FindProperty("_timeToFade");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_video);
            EditorGUILayout.PropertyField(_waitForFirstFrame);
            EditorGUILayout.PropertyField(_skipOnDrop);
            EditorGUILayout.PropertyField(_playbackSpeed);
            EditorGUILayout.PropertyField(_volume);
            EditorGUILayout.PropertyField(_mute);
            EditorGUILayout.PropertyField(_demoInactivityTime);
            EditorGUILayout.PropertyField(_timeToFade);
            serializedObject.ApplyModifiedProperties();
        }
    }
}