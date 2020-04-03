using UnityEditor;

namespace BUCK.LocalizationSystem.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(BaseDropdownLocalization), true)]
    public abstract class BaseDropdownLocalizationEditor : UILocalizationEditor
    {
        public override void OnInspectorGUI()
        {
            LocalizationEditor.GetKeys();
            DrawDefaultInspector();
            DrawTestingGUI();
        }
    }
}