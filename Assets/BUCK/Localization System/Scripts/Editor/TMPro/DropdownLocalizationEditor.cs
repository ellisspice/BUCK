#if TEXT_MESH_PRO_INCLUDED
using BUCK.LocalizationSystem.Editor;
using UnityEditor;
using TMPro;

namespace BUCK.LocalizationSystem.TMPro.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(DropdownLocalization), true)]
    public class DropdownLocalizationEditor : BaseDropdownLocalizationEditor
    {
        public override void Set()
        {
            Undo.RegisterCompleteObjectUndo(_myLoc.GetComponent<TMP_Dropdown>(), "Localization Override Change");
            base.Set();
        }
    }
}
#endif