# Localization System
## Description
A system for loading and managing localizing text throughout an application, with a set of components provided which allow for automatic setting of text on UI. 

## Example Usage
### Localization
```csharp
// Set-up the localization for this application if not already set-up.
void Localization.Initialize(string defaultLanguage = null);

// Clear the current localization settings and set-up application.
void Localization.ClearAndInitialize(string defaultLanguage = null);

// Load a localization file.
void Localization.AddLocalization(TextAsset textAsset);

// Parse text into information used for localization.
void Localization.AddLocalization(string text);

// Get the localized string for the provided key.
string Localization.Get(string key, bool toUpper = false, string overrideLanguage = null);

// Get the localized string for the provided key and insert any provided args into the string using string.Format.
string Localization.GetAndFormat(string key, params object[] args);

// Get the localized string for the provided key and insert any provided args into the string using string.Format.
string Localization.GetAndFormat(string key, bool toUpper, params object[] args);

// Get the localized string for the provided key and insert any provided args into the string using string.Format.
string Localization.GetAndFormat(string key, string overrideLanguage, bool toUpper, params object[] args);

// Get the localized string for the provided key and insert any provided args into the string using string.Format.
string Localization.GetAndFormat(string key, params string[] args);

// Get the localized string for the provided key and insert any provided args into the string using string.Format.
string Localization.GetAndFormat(string key, bool toUpper, params string[] args);

// Get the localized string for the provided key and insert any provided args into the string using string.Format.
string Localization.GetAndFormat(string key, string overrideLanguage, bool toUpper, params string[] args);

// Is this key valid for the currently selected language?
bool Localization.HasKey(string key);

// Get a list of all keys that can be used for localization.
List<string> Localization.Keys();

// Convert a string into the proper formatting for a localization key.
string Localization.KeyFormat(string key);

// Update the language to fallback to.
void Localization.SetDefaultCulture(string defaultLanguage);

// Set the language to use throughout the application.
void Localization.UpdateLanguage(string languageName);

// Set the language to use throughout the application.
void Localization.UpdateLanguage(CultureInfo language);

// Force the LanguageChange event to be fired without a change of language.
void Localization.OnLanguageChange();
```

### UILocalization
```csharp
// Update the associated UI Component on this GameObject.
void Set();
```

### BaseDropdownLocalization
```csharp
// Add options to the Options list.
void AddOptions(IEnumerable<LocalizationOptionData> options);

// Add options to the Options list.
void AddOptions(IEnumerable<string> options);

// Add options to the Options list.
void AddOptions(IEnumerable<Sprite> options);

// Clear the current options and add provided options to the Options list.
void ReplaceOptions(IEnumerable<LocalizationOptionData> options);

// Clear the current options and add provided options to the Options list.
void ReplaceOptions(IEnumerable<string> options);

// Clear the current options and add provided options to the Options list.
void ReplaceOptions(IEnumerable<Sprite> options);

// Clear the current options.
void ClearOptions();
```

### BaseTextLocalization
```csharp
// Update localization values.
void Set(string key, bool toUpper, params object[] args);

// Update the args that are inserted into the localized string.
void SetArgs(params object[] args);

// Update a single arg that will be inserted into the localized string.
void etArg(int index, object arg);
```