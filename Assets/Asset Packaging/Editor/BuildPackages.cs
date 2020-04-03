#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BUCK.AssetPackaging
{
    public static class BuildScript
    {
        [MenuItem("Tools/Packaging/Build Packages")]
        public static void ExportFiles()
        {
            var folders = Directory.GetDirectories(Application.dataPath + "/BUCK", "*.*");
            const ExportPackageOptions exportFlags = ExportPackageOptions.Recurse;
            foreach (var folder in folders)
            {
                var assetFiles = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                var assetExportLoc = Application.dataPath + "/../Builds/BUCK " + folder.Replace("\\", "/").Replace(Application.dataPath + "/BUCK/", string.Empty) + ".unitypackage";
                for (var i = 0; i < assetFiles.Length; i++)
                {
                    assetFiles[i] = "Assets/BUCK" + assetFiles[i].Replace("\\", "/").Replace(Application.dataPath + "/BUCK", string.Empty);
                }
                AssetDatabase.ExportPackage(assetFiles, assetExportLoc, exportFlags);
            }
            var files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
            var exportLoc = Application.dataPath + "/../Builds/BUCK All Assets.unitypackage";
            for (var i = 0; i < files.Length; i++)
            {
                files[i] = "Assets/BUCK" + files[i].Replace("\\", "/").Replace(Application.dataPath+ "/BUCK", string.Empty);
            }
            AssetDatabase.ExportPackage(files, exportLoc, exportFlags);
        }
    }
}
#endif