using UnityEditor;

namespace BUCK.VideoUI.UnityUI.Editor
{ 
    [CanEditMultipleObjects, CustomEditor(typeof(VideoPlaylistPlayerUI), true)]
    public class VideoPlaylistPlayerUIEditor : UnityEditor.Editor 
    {
        protected SerializedProperty _videoPlaylist;
        protected SerializedProperty _playOnAwake;
        protected SerializedProperty _waitForFirstFrame;
        protected SerializedProperty _loop;
        protected SerializedProperty _skipOnDrop;
        protected SerializedProperty _playbackSpeed;
        protected SerializedProperty _volume;
        protected SerializedProperty _mute;
        protected SerializedProperty _playButton;
        protected SerializedProperty _pauseButton;
        protected SerializedProperty _muteButton;
        protected SerializedProperty _unmuteButton;
        protected SerializedProperty _loopButton;
        protected SerializedProperty _unloopButton;
        protected SerializedProperty _positionSlider;
        protected SerializedProperty _volumeSlider;
        protected SerializedProperty _playbackDropdown;
        protected SerializedProperty _timer;
        
        protected virtual void OnEnable()
        {
            _videoPlaylist = serializedObject.FindProperty("_videoPlaylist");
            _playOnAwake = serializedObject.FindProperty("_playOnAwake");
            _waitForFirstFrame = serializedObject.FindProperty("_waitForFirstFrame");
            _loop = serializedObject.FindProperty("_loop");
            _skipOnDrop = serializedObject.FindProperty("_skipOnDrop");
            _playbackSpeed = serializedObject.FindProperty("_playbackSpeed");
            _volume = serializedObject.FindProperty("_volume");
            _mute = serializedObject.FindProperty("_mute");
            _playButton = serializedObject.FindProperty("_playButton");
            _pauseButton = serializedObject.FindProperty("_pauseButton");
            _muteButton = serializedObject.FindProperty("_muteButton");
            _unmuteButton = serializedObject.FindProperty("_unmuteButton");
            _loopButton = serializedObject.FindProperty("_loopButton");
            _unloopButton = serializedObject.FindProperty("_unloopButton");
            _positionSlider = serializedObject.FindProperty("_positionSlider");
            _volumeSlider = serializedObject.FindProperty("_volumeSlider");
            _playbackDropdown = serializedObject.FindProperty("_playbackDropdown");
            _timer = serializedObject.FindProperty("_timer");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_videoPlaylist, true);
            EditorGUILayout.PropertyField(_playOnAwake);
            EditorGUILayout.PropertyField(_waitForFirstFrame);
            EditorGUILayout.PropertyField(_loop);
            EditorGUILayout.PropertyField(_skipOnDrop);
            EditorGUILayout.PropertyField(_playbackSpeed);
            EditorGUILayout.PropertyField(_volume);
            EditorGUILayout.PropertyField(_mute);
            EditorGUILayout.PropertyField(_playButton);
            EditorGUILayout.PropertyField(_pauseButton);
            EditorGUILayout.PropertyField(_muteButton);
            EditorGUILayout.PropertyField(_unmuteButton);
            EditorGUILayout.PropertyField(_loopButton);
            EditorGUILayout.PropertyField(_unloopButton);
            EditorGUILayout.PropertyField(_positionSlider);
            EditorGUILayout.PropertyField(_volumeSlider);
            EditorGUILayout.PropertyField(_playbackDropdown);
            EditorGUILayout.PropertyField(_timer);
            serializedObject.ApplyModifiedProperties();
        }
    }
}