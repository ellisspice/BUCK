using UnityEngine;
using UnityEditor;

public class ExcelToJsonConverterWindow : EditorWindow 
{
	public const string ExcelToJsonConverterInputPathPrefsName = "ExcelToJson.InputPath";
	public const string ExcelToJsonConverterOutputPathPrefsName = "ExcelToJson.OutputPath";
	public const string ExcelToJsonConverterModifiedFilesOnlyPrefsName = "ExcelToJson.OnlyModifiedFiles";

	private string _inputPath;
	private string _outputPath;
	private bool _onlyModifiedFiles;

	private ExcelToJsonConverter _excelProcessor;

	[MenuItem ("Tools/BUCK/Localization/Excel To Json Converter")]
	public static void ShowWindow() 
	{
		GetWindow(typeof(ExcelToJsonConverterWindow), true, "Excel To Json Converter", true);
	}

	public void OnEnable()
	{
		if (_excelProcessor == null)
		{
			_excelProcessor = new ExcelToJsonConverter();
		}

		_inputPath = EditorPrefs.GetString(ExcelToJsonConverterInputPathPrefsName, Application.dataPath);
		_outputPath = EditorPrefs.GetString(ExcelToJsonConverterOutputPathPrefsName, Application.dataPath);
		_onlyModifiedFiles = EditorPrefs.GetBool(ExcelToJsonConverterModifiedFilesOnlyPrefsName, false);
	}
	
	public void OnDisable()
	{
		EditorPrefs.SetString(ExcelToJsonConverterInputPathPrefsName, _inputPath);
		EditorPrefs.SetString(ExcelToJsonConverterOutputPathPrefsName, _outputPath);
		EditorPrefs.SetBool(ExcelToJsonConverterModifiedFilesOnlyPrefsName, _onlyModifiedFiles);
	}

	private void OnGUI()
	{
		GUILayout.BeginHorizontal();

		var inputFolderContent = new GUIContent("Input Folder", "Select the folder where the excel files to be processed are located.");
		EditorGUIUtility.labelWidth = 120.0f;
		EditorGUILayout.TextField(inputFolderContent, _inputPath, GUILayout.MinWidth(240), GUILayout.MaxWidth(750));
		if (GUILayout.Button(new GUIContent("Select Folder"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
		{
			_inputPath = EditorUtility.OpenFolderPanel("Select Folder with Excel Files", _inputPath, Application.dataPath);
		}

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		var outputFolderContent = new GUIContent("Output Folder", "Select the folder where the converted json files should be saved.");
		EditorGUILayout.TextField(outputFolderContent, _outputPath, GUILayout.MinWidth(240), GUILayout.MaxWidth(750));
		if (GUILayout.Button(new GUIContent("Select Folder"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
		{
			_outputPath = EditorUtility.OpenFolderPanel("Select Folder to save json files", _outputPath, Application.dataPath);
		}
		
		GUILayout.EndHorizontal();

		var modifiedToggleContent = new GUIContent("Modified Files Only", "If checked, only excel files which have been newly added or updated since the last conversion will be processed.");
		_onlyModifiedFiles = EditorGUILayout.Toggle(modifiedToggleContent, _onlyModifiedFiles);

		if (string.IsNullOrEmpty(_inputPath) || string.IsNullOrEmpty(_outputPath))
		{
			GUI.enabled = false;
		}

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Convert Excel Files"))
		{
			_excelProcessor.ConvertExcelFilesToJson(_inputPath, _outputPath, _onlyModifiedFiles);
		}

		GUILayout.EndHorizontal();

		GUI.enabled = true;
	}
}

[InitializeOnLoad]
public class ExcelToJsonAutoConverter 
{	
	/// <summary>
	/// Class attribute [InitializeOnLoad] triggers calling the static constructor on every refresh.
	/// </summary>
	static ExcelToJsonAutoConverter() 
	{
		var inputPath = EditorPrefs.GetString(ExcelToJsonConverterWindow.ExcelToJsonConverterInputPathPrefsName, Application.dataPath);
		var outputPath = EditorPrefs.GetString(ExcelToJsonConverterWindow.ExcelToJsonConverterOutputPathPrefsName, Application.dataPath);
		var onlyModifiedFiles = EditorPrefs.GetBool(ExcelToJsonConverterWindow.ExcelToJsonConverterModifiedFilesOnlyPrefsName, false);
		
		var excelProcessor = new ExcelToJsonConverter();
		excelProcessor.ConvertExcelFilesToJson(inputPath, outputPath, onlyModifiedFiles);
	}
}
