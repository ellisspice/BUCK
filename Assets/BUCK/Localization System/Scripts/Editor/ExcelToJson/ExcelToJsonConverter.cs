using UnityEngine;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using ExcelDataReader;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class ExcelToJsonConverter 
{	
	public delegate void ConversionToJsonSuccessfulHandler();
	public event ConversionToJsonSuccessfulHandler ConversionToJsonSuccessful = delegate {};
	
	public delegate void ConversionToJsonFailedHandler();
	public event ConversionToJsonFailedHandler ConversionToJsonFailed = delegate {};
	
	/// <summary>
	/// Converts all excel files in the input folder to json and saves them in the output folder.
	/// Each sheet within an excel file is saved to a separate json file with the same name as the sheet name.
	/// Files, sheets and columns whose name begin with '~' are ignored.
	/// </summary>
	/// <param name="inputPath">Input path.</param>
	/// <param name="outputPath">Output path.</param>
	/// <param name="recentlyModifiedOnly">If set to <c>true</c>, will only process recently modified files only.</param>
	public void ConvertExcelFilesToJson(string inputPath, string outputPath, bool recentlyModifiedOnly = false)
	{
		var excelFiles = GetExcelFileNamesInDirectory(inputPath);
		Debug.Log("Excel To Json Converter: " + excelFiles.Count + " excel files found.");
		
		if (recentlyModifiedOnly)
		{
			excelFiles = RemoveUnmodifiedFilesFromProcessList(excelFiles, outputPath);
			
			if (excelFiles.Count == 0)
			{
				Debug.Log("Excel To Json Converter: No updates to excel files since last conversion.");
			}
			else
			{
				Debug.Log("Excel To Json Converter: " + excelFiles.Count + " excel files updated/added since last conversion.");
			}
		}
		
		var succeeded = excelFiles.All(file => ConvertExcelFileToJson(file, outputPath));

		if (succeeded)
		{
			ConversionToJsonSuccessful();
		}
		else
		{
			ConversionToJsonFailed();
		}
	}
	
	/// <summary>
	/// Gets all the file names in the specified directory
	/// </summary>
	/// <returns>The excel file names in directory.</returns>
	/// <param name="directory">Directory.</param>
	private static List<string> GetExcelFileNamesInDirectory(string directory)
	{
		var directoryFiles = Directory.GetFiles(directory);
		var excelFiles = new List<string>();
		
		// Regular expression to match against 2 excel file types (xls & xlsx), ignoring
		// files with extension .meta and starting with ~$ (temp file created by excel when fie
		var excelRegex = new Regex(@"^((?!(~\$)).*\.(xlsx|xls$))$");
		
		foreach (var file in directoryFiles)
		{
			var fileName = file.Substring(file.LastIndexOf('/') + 1);
			
			if (excelRegex.IsMatch(fileName))
			{
				excelFiles.Add(file);
			}
		}
		
		return excelFiles;
	}
	
	/// <summary>
	/// Converts each sheet in the specified excel file to json and saves them in the output folder.
	/// The name of the processed json file will match the name of the excel sheet. Ignores
	/// sheets whose name begin with '~'. Also ignores columns whose names begin with '~'.
	/// </summary>
	/// <returns><c>true</c>, if excel file was successfully converted to json, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	/// <param name="outputPath">Output path.</param>
	private static bool ConvertExcelFileToJson(string filePath, string outputPath)
	{
		Debug.Log("Excel To Json Converter: Processing: " + filePath);
		var excelData = GetExcelDataSet(filePath);
		
		if (excelData == null)
		{
			Debug.LogError("Excel To Json Converter: Failed to process file: " + filePath);
			return false;
		}

		// Process Each SpreadSheet in the excel file
		for (var i = 0; i < excelData.Tables.Count; i++)
		{
			var spreadSheetJson = GetSpreadSheetJson(excelData, excelData.Tables[i].TableName);
			if (string.IsNullOrEmpty(spreadSheetJson))
			{
				Debug.LogError("Excel To Json Converter: Failed to covert Spreadsheet '" + excelData.Tables[i].TableName + "' to json.");
				return false;
			}
			// The file name is the sheet name with spaces removed
			var fileName = excelData.Tables[i].TableName.Replace(" ", string.Empty);
			WriteTextToFile(spreadSheetJson, outputPath + "/" + fileName + ".json");
			Debug.Log("Excel To Json Converter: " + excelData.Tables[i].TableName + " successfully written to file.");
		}
		
		return true;
	}
	
	/// <summary>
	/// Gets the excel data reader for the specified file.
	/// </summary>
	/// <returns>The excel data reader for file or null if file type is invalid.</returns>
	/// <param name="filePath">File path.</param>
	private static IExcelDataReader GetExcelDataReaderForFile(string filePath)
	{
		var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		
		// Create the excel data reader
		IExcelDataReader excelReader;
		
		// Create regular expressions to detect the type of excel file
		var xlsRegex = new Regex(@"^(.*\.(xls$))");
		var xlsxRegex = new Regex(@"^(.*\.(xlsx$))");
		
		// Read the excel file depending on it's type
		if (xlsRegex.IsMatch(filePath))
		{
			// Reading from a binary Excel file ('97-2003 format; *.xls)
			excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		}
		else if (xlsxRegex.IsMatch(filePath))
		{
			// Reading from a OpenXml Excel file (2007 format; *.xlsx)
			excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		}
		else
		{
			Debug.LogError("Excel To Json Converter: Unexpected files type: " + filePath);
			stream.Close();
			return null;
		}

		return excelReader;
	}
	
	/// <summary>
	/// Gets the Excel data from the specified file
	/// </summary>
	/// <returns>The excel data set or null if file is invalid.</returns>
	/// <param name="filePath">File path.</param>
	private static DataSet GetExcelDataSet(string filePath)
	{
		// Get the excel data reader with the excel data
		var excelReader = GetExcelDataReaderForFile(filePath);

		// Get the data from the excel file
		var data = excelReader?.AsDataSet(new ExcelDataSetConfiguration
		{
			ConfigureDataTable = _ => new ExcelDataTableConfiguration
			{
				UseHeaderRow = true
			}
		});
		
		excelReader?.Close();
		
		return data;
	}

	/// <summary>
	/// Gets the json data for the specified spreadsheet in the specified DataSet
	/// </summary>
	/// <returns>The spread sheet json.</returns>
	/// <param name="excelDataSet">Excel data set.</param>
	/// <param name="sheetName">Sheet name.</param>
	private static string GetSpreadSheetJson(DataSet excelDataSet, string sheetName)
	{
		// Get the specified table
		var dataTable = excelDataSet.Tables[sheetName];
		
		// Remove empty columns
		for (var col = dataTable.Columns.Count - 1; col >= 0; col--)
		{
			var removeColumn = dataTable.Rows.Cast<DataRow>().All(row => row.IsNull(col));

			if (removeColumn)
			{
				dataTable.Columns.RemoveAt(col);
			}
		}
		
		// Remove columns which start with '~'
		var columnNameRegex = new Regex(@"^~.*$");
		for (var i = dataTable.Columns.Count - 1; i >= 0; i--)
		{
			if (columnNameRegex.IsMatch(dataTable.Columns[i].ColumnName))
			{
				dataTable.Columns.RemoveAt(i);
			}
		}

		var dataDictionaries = new List<Dictionary<string, string>>();

		foreach (DataRow row in dataTable.Rows)
		{
			var dict = dataTable.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => row[column.ColumnName].ToString());
			dataDictionaries.Add(dict);
		}

		// Serialize the data table to json string
		return JsonConvert.SerializeObject(dataDictionaries, Formatting.Indented);
	}
	
	/// <summary>
	/// Writes the specified text to the specified file, overwriting it.
	/// Creates file if it does not exist.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="filePath">File path.</param>
	private static void WriteTextToFile(string text, string filePath)
	{
		File.WriteAllText(filePath, text);
	}
	
	/// <summary>
	/// Removes files which have not been modified since they were last processed
	/// from the process list
	/// </summary>
	/// <param name="excelFiles">Excel files.</param>
	/// <param name="outputDirectory">Output dictionary to check against.</param>
	private static List<string> RemoveUnmodifiedFilesFromProcessList(List<string> excelFiles, string outputDirectory)
	{
		// ignore sheets whose name starts with '~'
		var sheetNameRegex = new Regex(@"^~.*$");
		
		for (var i = excelFiles.Count - 1; i >= 0; i--)
		{
			var sheetNames = GetSheetNamesInFile(excelFiles[i]);
			var removeFile = true;
			
			foreach (var name in sheetNames)
			{
				if (sheetNameRegex.IsMatch(name))
				{
					continue;
				}
				
				var outputFile = outputDirectory + "/" + name + ".json";
				if (!File.Exists(outputFile) ||
				    File.GetLastWriteTimeUtc(excelFiles[i]) > File.GetLastWriteTimeUtc(outputFile))
				{
					removeFile = false;
				}
			}
			
			if (removeFile)
			{
				excelFiles.RemoveAt(i);
			}
		}
		
		return excelFiles;
	}
	
	/// <summary>
	/// Gets the list of sheet names in the specified excel file
	/// </summary>
	/// <returns>The sheet names in file.</returns>
	/// <param name="filePath">File path.</param>
	private static IEnumerable<string> GetSheetNamesInFile(string filePath)
	{
		var sheetNames = new List<string>();
		
		// Get the excel data reader with the excel data
		var excelReader = GetExcelDataReaderForFile(filePath);
		
		if (excelReader == null)
		{
			return sheetNames;
		}
		
		do
		{
			// Add the sheet name to the list
			sheetNames.Add(excelReader.Name);
		}
		while(excelReader.NextResult()); // Read the next sheet
		
		excelReader.Close();
		return sheetNames;
	}
}
