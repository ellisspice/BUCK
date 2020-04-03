#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BUCK.LocalizationSystem.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(UILocalization), true)]
    
    public class UILocalizationEditor : UnityEditor.Editor
    {
        protected bool _testingOut;
        protected UILocalization _myLoc;

        protected virtual void Awake()
        {
            _myLoc = (UILocalization)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawTestingGUI();
        }

        public virtual void DrawTestingGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                LocalizationEditor.GetLanguages();
                if (LocalizationEditor.Keys.Count != 0 && LocalizationEditor.Languages.Count != 0)
                {
                    _testingOut = EditorGUILayout.Foldout(_testingOut, "Localization Testing");
                    if (_testingOut)
                    {
                        var index = Mathf.Max(0, LocalizationEditor.Languages.FindIndex(lang => _myLoc.LanguageOverride == lang.Name));

                        EditorGUI.BeginChangeCheck();
                        var overrideLang = LocalizationEditor.Languages[EditorGUILayout.Popup("Language Override", index, LocalizationEditor.Languages.Select(lang => lang.EnglishName).ToArray())].Name;
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RegisterCompleteObjectUndo(_myLoc, "Localization Override Change");
                            _myLoc.LanguageOverride = overrideLang;
                        }
                        if (GUILayout.Button("Localize"))
                        {
                            Set();
                        }
                    }
                }
            }
        }

        public virtual void Set()
        {
            _myLoc.Set();
        }
    }
}