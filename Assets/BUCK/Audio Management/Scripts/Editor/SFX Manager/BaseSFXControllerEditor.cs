using UnityEditor;

namespace BUCK.AudioManagement.SFX.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(BaseSFXController), true)]
    public class BaseSFXControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector ();
            serializedObject.ApplyModifiedProperties();
        }
    }
}