#if TEXT_MESH_PRO_INCLUDED
using BUCK.LocalizationSystem.Editor;
using TMPro;
using UnityEditor;

namespace BUCK.LocalizationSystem.TMPro.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(TextLocalization), true)]
    public class TextLocalizationEditor : BaseTextLocalizationEditor
    {
        public override void Set()
        {
            Undo.RegisterCompleteObjectUndo(_myLoc.GetComponent<TextMeshProUGUI>(), "Localization Override Change");
            base.Set();
        }
    }
}
#endif