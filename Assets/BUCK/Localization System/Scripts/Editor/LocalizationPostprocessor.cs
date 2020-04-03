#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Linq;
using UnityEditor;

namespace BUCK.LocalizationSystem.Editor
{
    public class LocalizationPostprocessor : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var assets = importedAssets.ToList();
            assets.AddRange(deletedAssets);
            assets.AddRange(movedAssets);
            assets.AddRange(movedFromAssetPaths);

            if (assets.Any(a => a.Contains("/Resources/" + Localization.FilePath)))
            {
                LocalizationEditor.GetKeys(true);
            }
        }
    }
}