using UnityEditor;
using UnityEngine;

namespace BUCK.LocalizationSystem.Editor
{
	public class LocalizationEditorWindow : EditorWindow
	{
		private const string OutputPathSavedName = "Localization Output";
		private string _outputPath;

		[MenuItem("Tools/BUCK/Localization/Generate Constant Files")]
		public static void ShowWindow()
		{
			GetWindow(typeof(LocalizationEditorWindow), true, "Generate Localization Constant Files", true);
		}

		public void OnEnable()
		{
			_outputPath = EditorPrefs.GetString(OutputPathSavedName, Application.dataPath);
		}

		public void OnDisable()
		{
			EditorPrefs.SetString(OutputPathSavedName, _outputPath);
		}

		private void OnGUI()
		{
			EditorGUIUtility.labelWidth = 120.0f;
			GUILayout.BeginHorizontal();
			var outputFolder = new GUIContent("Output Folder", "Folder to save generated files in.");
			EditorGUILayout.TextField(outputFolder, _outputPath, GUILayout.MinWidth(240), GUILayout.MaxWidth(750));
			if (GUILayout.Button(new GUIContent("Select Folder"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
			{
				_outputPath = EditorUtility.OpenFolderPanel("Select folder to save generated files", _outputPath, Application.dataPath);
			}
			GUILayout.EndHorizontal();
			if (string.IsNullOrEmpty(_outputPath))
			{
				GUI.enabled = false;
			}
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Generate Constant Files"))
			{
				LocalizationEditor.GenerateConstFile(_outputPath);
			}
			GUILayout.EndHorizontal();

			GUI.enabled = true;
		}
	}
}