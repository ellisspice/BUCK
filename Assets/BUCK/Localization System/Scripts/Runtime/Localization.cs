#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
#if BEST_FIT_TEXT_INCLUDED
using BUCK.BestFitText;
#endif

namespace BUCK.LocalizationSystem
{
    /// <summary>
    /// Localization manager.
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Name of the default language to fallback to if other valid options are not available.
        /// </summary>
        private static string DefaultLanguageName { get; set; } = "en";
        /// <summary>
        /// StringComparer used throughout to ignore case when comparing.
        /// </summary>
        private static readonly StringComparer IgnoreCase = StringComparer.OrdinalIgnoreCase;
        /// <summary>
        /// Array of all NeutralCultures.
        /// </summary>
        private static readonly CultureInfo[] NeutralCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
        /// <summary>
        /// Name of the folder in Resources that contains localization json files.
        /// </summary>
        public const string FilePath = "Localization";
        /// <summary>
        /// Key used to save and load the last language used in PlayerPrefs.
        /// </summary>
        public const string SavedLanguageKey = "Last_Saved_Language";
        
        /// <summary>
        /// Dictionary of all languages and their respective dictionary of localization keys and values.
        /// </summary>
        private static readonly Dictionary<CultureInfo, Dictionary<string, string>> LocalizationDict = new Dictionary<CultureInfo, Dictionary<string, string>>();
        /// <summary>
        /// Default language to fallback to if other valid options are not available.
        /// </summary>
        private static CultureInfo DefaultLanguage { get; set; }
        
        /// <summary>
        /// List of all available languages.
        /// </summary>
        public static readonly List<CultureInfo> Languages = new List<CultureInfo>();
        /// <summary>
        /// The language currently being used.
        /// </summary>
        public static CultureInfo SelectedLanguage { get; private set; }
        /// <summary>
        /// The specific version of the language currently being used.
        /// </summary>
        public static CultureInfo SpecificSelectedLanguage { get; private set; }
        /// <summary>
        /// Event triggered whenever SelectedLanguage is changed.
        /// </summary>
        public static event Action LanguageChange = delegate { };

        static Localization()
        {
            SceneManager.activeSceneChanged += UpdateAllUI;
            Initialize();
        }
        
        /// <summary>
        /// Set-up the localization for this application if not already set-up.
        /// </summary>
        /// <param name="defaultLanguage">Optional. Language to fallback to if other valid options are not available.</param>
        public static void Initialize(string defaultLanguage = null)
        {
            if (Languages.Count == 0)
            {
                GetLocalizationDictionary(true, defaultLanguage);
            }
        }

        /// <summary>
        /// Clear the current localization settings and set-up application.
        /// </summary>
        /// <param name="defaultLanguage">Optional. Language to fallback to if other valid options are not available.</param>
        public static void ClearAndInitialize(string defaultLanguage = null)
        {
            LocalizationDict.Clear();
            Languages.Clear();
            SelectedLanguage = null;
            Initialize(defaultLanguage);
        }

        /// <summary>
        /// Load localization files and set SelectedLanguage based on previously used language or device settings.
        /// </summary>
        /// <param name="loadLang">Optional. Should the SelectedLanguage be set?</param>
        /// <param name="defaultLanguage">Optional. Language to fallback to if other valid options are not available.</param>
        private static void GetLocalizationDictionary(bool loadLang = true, string defaultLanguage = null)
        {
            var jsonTextAssets = Resources.LoadAll<TextAsset>(FilePath);
            foreach (var textAsset in jsonTextAssets)
            {
                AddLocalization(textAsset);
            }
            SetDefaultCulture(defaultLanguage ?? DefaultLanguageName);
            if (loadLang)
            {
                UpdateLanguage(PlayerPrefs.GetString(SavedLanguageKey));
                if (SelectedLanguage == null)
                {
                    UpdateLanguage(GetLanguage(CultureInfo.CurrentUICulture));
                }
                if (SelectedLanguage == null)
                {
                    UpdateLanguage(GetLanguage(CultureInfo.CurrentCulture));
                }
                if (SelectedLanguage == null)
                {
                    UpdateLanguage(GetLanguage(GetFromSystemLanguage()));
                }
                if (SelectedLanguage == null)
                {
                    UpdateLanguage(GetLanguage(DefaultLanguage));
                }
            }
        }

        /// <summary>
        /// Load a localization file.
        /// </summary>
        /// <param name="textAsset">File to load.</param>
        public static void AddLocalization(TextAsset textAsset)
        {
            AddLocalization(textAsset.text);
        }

        /// <summary>
        /// Parse text into information used for localization.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        public static void AddLocalization(string text)
        {
            var json = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(text);
            json = json.Select(d => new Dictionary<string, string>(d, IgnoreCase)).ToList();
            AddLanguages(json);
            AddToDict(json);
        }

        /// <summary>
        /// Get available language names from json keys and attempt to add them to available languages in application.
        /// </summary>
        /// <param name="json">Collection of dictionaries from which to get language names from.</param>
        private static void AddLanguages(IEnumerable<Dictionary<string, string>> json)
        {
            const StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;
            var keys = json.SelectMany(j => j.Keys.Where(k => !k.Equals("key", ignoreCase))).Distinct().ToList();
            foreach (var key in keys)
            {
                AddLanguage(key, out _);
            }
        }

        /// <summary>
        /// Add a language to the application if it valid or return the currently existing language if already added.
        /// </summary>
        /// <param name="languageName">Name of the language to add.</param>
        /// <param name="language">Newly gathered or already gathered CultureInfo for this language.</param>
        /// <returns>Was the language name provided a valid CultureInfo name?</returns>
        private static bool AddLanguage(string languageName, out CultureInfo language)
        {
            if (!string.IsNullOrEmpty(languageName))
            {
                var existing = Languages.Any() ? Languages.FirstOrDefault(l => l.Name == languageName) : null;
                if (existing != null)
                {
                    language = existing;
                    return true;
                }
                if (TryGetCultureInfo(languageName, out language))
                {
                    Languages.Add(language);
                    LocalizationDict.Add(language, new Dictionary<string, string>(IgnoreCase));
                    return true;
                }
            }
            language = null;
            return false;
        }

        /// <summary>
        /// Add a language to the application if it valid or return the currently existing language if already added.
        /// </summary>
        /// <param name="languageName">Name of the language to add.</param>
        /// <returns>Was the language name provided a valid CultureInfo name?</returns>
        private static CultureInfo AddLanguage(string languageName)
        {
            AddLanguage(languageName, out var language);
            return language;
        }
        
        /// <summary>
        /// Try to create a CultureInfo using the language name provided.
        /// </summary>
        /// <param name="languageName">Name of the language to attempt to create a CultureInfo for.</param>
        /// <param name="language">CultureInfo for this language name.</param>
        /// <returns>Newly created or already created CultureInfo for this language.</returns>
        private static bool TryGetCultureInfo(string languageName, out CultureInfo language)
        {
            try
            {
                language = CultureInfo.GetCultureInfo(languageName);
                return true;
            }
            catch (CultureNotFoundException ex)
            {
                Debug.LogError(ex);
                language = null;
                return false;
            }
        }

        /// <summary>
        /// Add provided collection of dictionaries to the available keys and values for localization.
        /// </summary>
        /// <param name="json">Parsed json to add to localization dictionary.</param>
        private static void AddToDict(IEnumerable<Dictionary<string, string>> json)
        {
            foreach (var dict in json)
            {
                if (dict.TryGetValue("key", out var key))
                {
                    foreach (var l in Languages)
                    {
                        if (dict.TryGetValue(l.Name, out var str))
                        {
                            key = KeyFormat(key);
                            if (LocalizationDict[l].ContainsKey(key))
                            {
                                LocalizationDict[l][key] = str;
                            }
                            else
                            {
                                LocalizationDict[l].Add(key, str);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the localized string for the provided key.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="toUpper">Optional. Should the localized text be made all uppercase?</param>
        /// <param name="overrideLanguage">Optional. Name of the language to use for localization rather than the current SelectedLanguage.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string Get(string key, bool toUpper = false, string overrideLanguage = null)
        {
            if (SelectedLanguage == null)
            {
                GetLocalizationDictionary();
            }
            //if no key is provided, return an empty string
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            key = KeyFormat(key);
            if (!AddLanguage(overrideLanguage, out var language))
            {
                language = SelectedLanguage ?? DefaultLanguage;
            }
            LocalizationDict[language].TryGetValue(key, out var text);
            //Get the parent of the current language, to be used for further attempts to get text if none was gotten.
            var parentLang = !Equals(language.Parent, CultureInfo.InvariantCulture) ? language.Parent : language;
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning($"Could not find string with key '{key}' for Language {language.Name}");
                //Attempt to get the localized text for the parent of the current language if it is a valid option.
                if (!Equals(parentLang, language) && Languages.Contains(parentLang))
                {
                    LocalizationDict[parentLang].TryGetValue(key, out var newText);
                    if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(newText))
                    {
                        text = newText;
                    }
                }
            }
            //Attempt to get the localized text for any children of the parent of the current language.
            foreach (var lang in Languages)
            {
                if (string.IsNullOrEmpty(text) && Equals(lang.Parent, parentLang) && !Equals(lang, language))
                {
                    LocalizationDict[lang].TryGetValue(key, out var newText);
                    if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(newText))
                    {
                        text = newText;
                    }
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                //Attempt to get the localized text for the default language.
                LocalizationDict[DefaultLanguage].TryGetValue(key, out var newText);
                if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(newText))
                {
                    text = newText;
                }
                //As a last resort return the key.
                if (string.IsNullOrEmpty(text))
                {
                    text = key;
                }
            }
            //Format the text.
            text = text.Replace("\\n", "\n");
            text = text.Replace('[', '{');
            text = text.Replace(']', '}');
            if (toUpper)
            {
                text = text.ToUpper();
            }
            return text;
        }
        
        /// <summary>
        /// Get the localized string for the provided key and insert any provided args into the string using string.Format.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="args">Strings to insert into the localized text.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string GetAndFormat(string key, params object[] args)
        {
            return GetAndFormat(key, false, args);
        }
        
        /// <summary>
        /// Get the localized string for the provided key and insert any provided args into the string using string.Format.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="toUpper">Should the localized text be made all uppercase?</param>
        /// <param name="args">Strings to insert into the localized text.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string GetAndFormat(string key, bool toUpper, params object[] args)
        {
            return GetAndFormat(key, null, toUpper, args);
        }

        /// <summary>
        /// Get the localized string for the provided key and insert any provided args into the string using string.Format.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="overrideLanguage">Name of the language to use for localization rather than the current SelectedLanguage.</param>
        /// <param name="toUpper">Should the localized text be made all uppercase?</param>
        /// <param name="args">Strings to insert into the localized text.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string GetAndFormat(string key, string overrideLanguage, bool toUpper, params object[] args)
        {
            var text = Get(key, toUpper, overrideLanguage);
            try
            {
                return string.Format(text, args);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return text;
            }
        }
        
        /// <summary>
        /// Get the localized string for the provided key and insert any provided args into the string using string.Format.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="args">Strings to insert into the localized text.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string GetAndFormat(string key, params string[] args)
        {
            return GetAndFormat(key, false, args);
        }
        
        /// <summary>
        /// Get the localized string for the provided key and insert any provided args into the string using string.Format.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="toUpper">Should the localized text be made all uppercase?</param>
        /// <param name="args">Strings to insert into the localized text.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string GetAndFormat(string key, bool toUpper, params string[] args)
        {
            return GetAndFormat(key, null, toUpper, args);
        }

        /// <summary>
        /// Get the localized string for the provided key and insert any provided args into the string using string.Format.
        /// </summary>
        /// <param name="key">Key to get localized text for.</param>
        /// <param name="overrideLanguage">Name of the language to use for localization rather than the current SelectedLanguage.</param>
        /// <param name="toUpper">Should the localized text be made all uppercase?</param>
        /// <param name="args">Strings to insert into the localized text.</param>
        /// <returns>The localized string for the provided key.</returns>
        public static string GetAndFormat(string key, string overrideLanguage, bool toUpper, params string[] args)
        {
            return GetAndFormat(key, overrideLanguage, toUpper, args.ToArray<object>());
        }

        /// <summary>
        /// Is this key valid for the currently selected language?
        /// </summary>
        /// <param name="key">Key to check validity for.</param>
        /// <returns>Validity of the key.</returns>
        public static bool HasKey(string key)
        {
            return LocalizationDict[SelectedLanguage].ContainsKey(KeyFormat(key));
        }
        
        /// <summary>
        /// Get a list of all keys that can be used for localization.
        /// </summary>
        /// <returns>List of all localization keys.</returns>
        public static List<string> Keys()
        {
            var keyList = LocalizationDict.Values.SelectMany(l => l.Keys).Distinct().ToList();
            keyList.Sort();
            return keyList;
        }

        /// <summary>
        /// Convert a string into the proper formatting for a localization key.
        /// </summary>
        /// <param name="key">Key to update formatting for.</param>
        /// <returns>Correctly formatted key.</returns>
        public static string KeyFormat(string key)
        {
            return string.Concat(key.ToUpper().Replace('-', '_').Where(c => !char.IsWhiteSpace(c)));
        }

        /// <summary>
        /// Get the current Application.systemLanguage.
        /// </summary>
        /// <returns>CultureInfo (if any) being used for localization within this application which matches the English Name of the current Application.systemLanguage.</returns>
        private static CultureInfo GetFromSystemLanguage()
        {
            return NeutralCultures.FirstOrDefault(r => r.EnglishName == Application.systemLanguage.ToString());
        }

        /// <summary>
        /// Get the matching CultureInfo within the Languages list for the CultureInfo provided.
        /// </summary>
        /// <param name="language">CultureInfo for which to get the matching result in Languages.</param>
        /// <returns>Existing CultureInfo within the Languages list which matches the CultureInfo provided.</returns>
        private static CultureInfo GetLanguage(CultureInfo language)
        {
            if (language == null)
            {
                return null;
            }
            if (!Languages.Contains(language))
            {
                if (!Equals(language.Parent, CultureInfo.InvariantCulture))
                {
                    language = language.Parent;
                }
                if (!Languages.Contains(language))
                {
                    language = Languages.Where(c => Equals(c.Parent, language)).ToList().Count > 0 ? Languages.First(c => Equals(c.Parent, language)) : DefaultLanguage;
                }
            }
            return language;
        }

        /// <summary>
        /// Update the language to fallback to.
        /// </summary>
        /// <param name="defaultLanguage">Name of the language to use as a final final fallback.</param>
        public static void SetDefaultCulture(string defaultLanguage)
        {
            if (AddLanguage(defaultLanguage, out var language))
            {
                DefaultLanguageName = defaultLanguage;
                DefaultLanguage = language;
            }
        }
        
        /// <summary>
        /// Set the language to use throughout the application.
        /// </summary>
        /// <param name="languageName">Name of the language to use.</param>
        public static void UpdateLanguage(string languageName)
        {
            if (string.IsNullOrEmpty(languageName) || !AddLanguage(languageName, out var language))
            {
                return;
            }
            UpdateLanguage(language);
        }
        
        /// <summary>
        /// Set the language to use throughout the application.
        /// </summary>
        /// <param name="language">CultureInfo to use.</param>
        public static void UpdateLanguage(CultureInfo language)
        {
            if (Languages.Count == 0)
            {
                GetLocalizationDictionary(false);
            }
            AddLanguage(language.Name, out language);
            if (!Equals(language, SelectedLanguage) && language != null)
            {
                if (!Languages.Contains(language))
                {
                    var parentLang = !Equals(language.Parent, CultureInfo.InvariantCulture) ? language.Parent : language;
                    if (!Equals(parentLang, language) && Languages.Contains(parentLang))
                    {
                        language = parentLang;
                    }
                }
                if (!Languages.Contains(language))
                {
                    foreach (var lang in Languages)
                    {
                        if (Equals(lang.Parent, language))
                        {
                            language = lang;
                            break;
                        }
                    }
                }
                if (Languages.Contains(language))
                {
                    SelectedLanguage = language;
                    SpecificSelectedLanguage = SelectedLanguage.IsNeutralCulture ? CultureInfo.CreateSpecificCulture(SelectedLanguage.Name) : SelectedLanguage;
                    if (Application.isPlaying)
                    {
                        PlayerPrefs.SetString(SavedLanguageKey, language.Name);
                        UpdateAllUI();
                        OnLanguageChange();
                        Debug.Log(SelectedLanguage);
                    }
                }
            }
        }

        /// <summary>
        /// Fire the LanguageChange event.
        /// </summary>
        public static void OnLanguageChange()
        {
            LanguageChange?.Invoke();
#if BEST_FIT_TEXT_INCLUDED
            BestFit.OnResolutionChange();
#endif
        }
        
        /// <summary>
        /// Trigger all UI to be updated to display text which matches the currently selected language.
        /// </summary>
        private static void UpdateAllUI(Scene arg0, Scene arg1)
        {
            UpdateAllUI();
        }

        /// <summary>
        /// Trigger all UI to be updated to display text which matches the currently selected language.
        /// </summary>
        private static void UpdateAllUI()
        {
            Resources.FindObjectsOfTypeAll<UILocalization>().Where(l => l.gameObject.scene == SceneManager.GetActiveScene()).ToList().ForEach(l => l.Set());
        }
    }
}