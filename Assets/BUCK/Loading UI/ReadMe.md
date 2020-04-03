# Loading UI
## Description
A set of scripts and components related to displaying a loading screen using Unity's UI system.

The scripts provided allow for the simple creation of loading UI which can be accessed from anywhere in an application.

The static Loading class is the main access point, containing methods for setting loading values, text and displaying/hiding the UI.

The BaseLoadingUI is an abstract class containing all of the basic functionality needed for a loading UI, with versions provide for the standard Unity UI Text component and the TextMeshPro TMP_Text component. The LoadingSpinner and LoadingBar components inherit from the BaseLoadingUI class, with functionality adjusted to fit the needs of that type of loading UI.

## Example Usage
An example prefab and scene displaying usage of those prefabs have been provided for the LoadingSpinner and LoadingBar components, all of which can be found in the "Examples" folder. GameObjects similar to these prefabs can also be created via the "GameObject/UI" menu as well.

```csharp
// Set the value related to loading for the current UI.
void Loading.SetValue(float value = 0);

// Set the text displayed during loading.
void Loading.SetText(string text);

// Display the loading UI.
void Loading.Start(string text = "");

// Hide the loading UI.
void Loading.Stop(string text = "", float stopDelay = 0f);
```