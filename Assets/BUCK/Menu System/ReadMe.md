# Menu System
## Description
A collection of components and scripts relating to the creation of menus using Unity's UI systems.

### Arrow Button Selectable
The ArrowButtonSelectable script adds a component for selecting a value from a list using two buttons which represent going left and right through the list. This is designed for use as an alternative to the Dropdown and can be used for functionality such as changing volume and selecting screen resolution.

### Menu Controller
The Menu Controller set of scripts is designed for setting and showing the current UI state.

### Orientation Anchors
The OrientationAnchors script adds a component for setting min and max anchors depending on if the screen is currently in a landscape or portrait aspect ratio, as well as a set of editor tools for simplifying setting and testing these values.

### Selection Maintainer
The Selection Maintainer set of scripts is designed for ensuring a selectable is always focused on when controlling a menu with either a keyboard or a controller, with a slightly adapted InputField and Button included to make full use of this functionality. 
## Example Usage

A full generic menu has been provided in the "Examples" folder, designed to both use all of the provided menu assets to show off the functionality and to provide the basis of a menu which could be quickly implemented in a rapid prototyping environment. 

### Arrow Button Selectable
An example prefab for the ArrowButtonSelectable component has been provided within the "Examples" folder. A generic ArrowButtonSelectable for both Unity UUI and TextMeshPro can be created via the "GameObject/UI" menu.

To keep with the idea that the ArrowButtonSelectable is designed as an alternative to the Dropdown, it also uses much of the same functionality and naming standards regarding events, variables and methods as the Dropdown script.

### Orientation Anchors
To use the OrientationAnchors script, add it as a component to any object using a RectTransform (aka a UI object). Editor tools can be found in the context menu of the component or under the "Tools/Orientation Anchors" menu.

### Selection Maintainer
To use the Selection Maintainer script, add it as a component to the base object of your menu objects (most likely the Canvas itself). To get full support of the Selection Maintainer the SMButton and SMInputField components should be used instead of the default provided components.

### Menu Controller
#### MenuManager
```csharp
// Set the menu state to display
void SetState(MenuState activeState);

// Set the action to take when pressing the Back Input Key or Button in the current MenuState.
void SetBackAction(UnityAction action);
```

#### MenuBehaviour
```csharp
// Set the action to take when pressing the Back Input Key or Button in this behaviour.
void SetBackAction(UnityAction action);

// Trigger the Back Action.
void Back();
```