using UnityEditor;

namespace BUCK.AudioManagement.Music.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(BaseMusicManager), true)]
    public class BaseMusicManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector ();
            serializedObject.ApplyModifiedProperties();
        }
    }
}