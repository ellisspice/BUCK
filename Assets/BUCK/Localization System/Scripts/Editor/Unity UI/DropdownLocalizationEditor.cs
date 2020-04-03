using BUCK.LocalizationSystem.Editor;
using UnityEditor;
using UnityEngine.UI;

namespace BUCK.LocalizationSystem.UnityUI.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(DropdownLocalization), true)]
    public class DropdownLocalizationEditor : BaseDropdownLocalizationEditor
    {
        public override void Set()
        {
            Undo.RegisterCompleteObjectUndo(_myLoc.GetComponent<Dropdown>(), "Localization Override Change");
            base.Set();
        }
    }
}