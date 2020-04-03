#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BUCK.LocalizationSystem.Editor
{
    public static class LocalizationEditor
    {
        internal static List<CultureInfo> Languages;
        internal static List<string> Keys;

        internal static void GetKeys(bool reload = false)
        {
            if (reload || Keys == null || Keys.Count == 0)
            {
                GetLanguages(true);
                Keys = Localization.Keys();
            }
        }

        internal static void GetLanguages(bool reload = false)
        {
            if (reload || Languages == null || Languages.Count == 0)
            {
                Localization.ClearAndInitialize();
                Languages = Localization.Languages;
            }
        }

        [MenuItem("Tools/BUCK/Localization/Font Localization Character Check")]
        public static void FontLocalizationCharacterCheck()
        {
            GetKeys();
            var allFonts = Resources.FindObjectsOfTypeAll<Font>();
            var characterList = new HashSet<char>();
            foreach (var language in Languages)
            {
                foreach (var key in Keys)
                {
                    var normal = Localization.Get(key, false, language.Name).Where(c => !characterList.Contains(c));
                    foreach (var c in normal)
                    {
                        characterList.Add(c);
                    }
                    var upper = Localization.Get(key, true, language.Name).Where(c => !characterList.Contains(c));
                    foreach (var c in upper)
                    {
                        characterList.Add(c);
                    }
                }
            }
            Debug.Log("Unique Character Count: " + characterList.Count);
            foreach (var font in allFonts)
            {
                if (font.dynamic)
                {
                    Debug.LogWarning(font.name + " is currently set to be a dynamic font. Dynamic fonts use default fonts when characters aren't available, so can't be tested. Change this setting to test this font.");
                    continue;
                }
                var missingCharacters = characterList.Where(c => !font.HasCharacter(c)).ToList();
                if (missingCharacters.Count == 0)
                {
                    Debug.Log(font.name + " is missing no characters");
                }
                else
                {
                    var debugString = font.name + " is missing the following characters:";
                    debugString = missingCharacters.Aggregate(debugString, (current, c) => current + "\n" + c + " (Unicode Index:" + (int) c + ")");
                    Debug.LogWarning(debugString);
                }
            }
        }
        
        public static void GenerateConstFile(string outputPath)
        {
            GetKeys();
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var keys = Localization.Keys().Aggregate("namespace BUCK.LocalizationSystem\n{\n\tpublic class LocalizationKeys\n\t{\n" , (current, key) => current + "\t\tpublic const string " + textInfo.ToTitleCase(key.ToLower().Replace('_', ' ')).Replace(" ", string.Empty) + " = \"" + key + "\";\n");
            keys += "\t}\n}";
            WriteFile(Path.Combine(outputPath, "LocalizationKeys.cs"), keys);
            var languages = Localization.Languages.Aggregate("namespace BUCK.LocalizationSystem\n{\n\tpublic class LocalizationLanguages\n\t{\n" , (current, language) => current + "\t\tpublic const string " + textInfo.ToTitleCase(language.EnglishName).Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty) + " = \"" + language.Name + "\";\n");
            languages += "\t}\n}";
            WriteFile(Path.Combine(outputPath, "LocalizationLanguages.cs"), languages);
            AssetDatabase.Refresh();
        }
        
        public static void WriteFile(string file, string body)
        {
            file = SlashesToPlatformSeparator(file);
            File.WriteAllText(file, body);
        }
        
        public static string SlashesToPlatformSeparator(string path)
        {
            return path.Replace("/", Path.DirectorySeparatorChar.ToString());
        }
    }
}