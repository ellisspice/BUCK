#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using BUCK.LocalizationSystem.Editor;
using UnityEditor;
using UnityEngine.UI;

namespace BUCK.LocalizationSystem.UnityUI.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(TextLocalization), true)]
    public class TextLocalizationEditor : BaseTextLocalizationEditor
    {
        public override void Set()
        {
            Undo.RegisterCompleteObjectUndo(_myLoc.GetComponent<Text>(), "Localization Override Change");
            base.Set();
        }
    }
}