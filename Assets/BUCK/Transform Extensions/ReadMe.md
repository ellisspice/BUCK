# Transform Extensions
## Description
Set of extension methods for getting RectTransforms more easily and which adds additional functionality to existing Transform methods.

## Example Usage

```csharp
// Get the RectTransform for the Object.
RectTransform Transform.RectTransform();
RectTransform GameObject.RectTransform();
RectTransform Component.RectTransform();

// Find the child with the matching name and return the RectTransform.
RectTransform Transform.FindRect(string find);

// Find the child with the matching name and return Component T (if one exists) on that child.
T Transform.FindComponent<T>(string find);
T GameObject.FindComponent<T>(string find);
T Component.FindComponent<T>(string find);

// Find the child with the matching name and return the Image Component (if one exists) on that child.
Image Transform.FindImage(string find);
Image GameObject.FindImage(string find);
Image Component.FindImage(string find);

// Find the child with the matching name and return the Text Component (if one exists) on that child.
Text Transform.FindText(string find);
Text GameObject.FindText(string find);
Text Component.FindText(string find);

// Find the child with the matching name and return the TextMeshProUGUI Component (if one exists) on that child.
TextMeshProUGUI Transform.FindTMProText(string find);
TextMeshProUGUI GameObject.FindTMProText(string find);
TextMeshProUGUI Component.FindTMProText(string find);

// Find the child with the matching name and return the Button Component (if one exists) on that child.
Button Transform.FindButton(string find);
Button GameObject.FindButton(string find);
Button Component.FindButton(string find);

// Find the child with the matching name and return the GameObject of that child.
GameObject Transform.FindObject(string find);
GameObject GameObject.FindObject(string find);
GameObject Component.FindObject(string find);

// Return the parent GameObject (if any) of the object.
GameObject Transform.Parent();
GameObject GameObject.Parent();
GameObject Component.Parent();

// Find the child with the matching name and return Component T on that child or its children, if one exists and is accessiblle depending on the includeInactive setting.
T Transform.FindComponentInChildren<T>(string find, bool includeInactive = false);
T GameObject.FindComponentInChildren<T>(string find, bool includeInactive = false);
T Component.FindComponentInChildren<T>(string find, bool includeInactive = false);
```