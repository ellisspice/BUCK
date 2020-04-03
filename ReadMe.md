# Basic Unity Component Kit (BUCK)
A collection of Unity packages designed to allow for the quick implementation of common functionality.

## Included Assets
- [**Audio Management**](Assets/BUCK/Audio%20Management) - A set of managers and controllers which are used to handle the playing soundtrack and sound effect audio and to control audio volume.
- [**Best Fit Text**](Assets/BUCK/Best%20Fit%20Text) - Utilizes the built-in Text Component "Best Fit" feature to make a set of Text Components a uniform size based on the size of the smallest involved in that calculation.
- [**Camera Controllers**](Assets/BUCK/Camera%20Controllers) - A set of components related to limiting the aspect ratio or width of an orthographic Camera.
- [**Common Coroutine Controller**](Assets/BUCK/Common%20Coroutine%20Controller) - A singleton component designed to be called when Coroutine functionality is required within a non-MonoBehaviour script.
- [**Loading UI**](Assets/BUCK/Loading%20UI) - A set of scripts and components related to displaying a loading screen using Unity's UI system.
- [**Localization System**](Assets/BUCK/Localization%20System) - A system for loading and managing localizing text throughout an application, with a set of components provided which allow for automatic setting of text on UI.
- [**Menu System**](Assets/BUCK/Menu%20System) - A collection of components and scripts relating to the creation of menus using Unity's UI systems.
- [**Profile System**](Assets/BUCK/Profile%20System) - A system for saving and loading data for multiple users in a single application.
- [**Transform Extensions**](Assets/BUCK/Transform%20Extensions) - Set of extension methods for getting RectTransforms more easily and which adds additional functionality to existing Transform methods.
- [**Video UI**](Assets/BUCK/Video%20UI) - A set of components related to playing videos within Unity's UI system.

## Usage
There are multiple different ways to add BUCK (or any of the ten BUCK asset collections) to your project:
- Copy the "BUCK" folder (or the folder containing the asset you wish to add) into the "Assets" folder of your Unity project.
- Copy the "BUCK" folder (or the folder containing the asset you wish to add) into the "Packages" folder of your Unity project. This will also add any required dependencies into your project.
- Add the "BUCK" folder (or the folder containing the asset you wish to add) using the "Add package from disk" option within the Package Manager.
- Either using the provided unitypackage within the "Builds" folder or by generating your own using the "Tools/Packaging/Build Packages" menu option, drag and drop the desired unitypackage into your project.

## Design Decisions
All of the assets within BUCK have been designed to be as easily implementable and adaptable as possible. As such, there are no private methods, fields or properties within BUCK and all methods have the virtual keyword (bar static classes, where this is not possible) so that everything is available within any classes that inherit from a BUCK class. Furthermore, all code source for all BUCK assets is freely available and editable, allowing for any changes to be able to the source itself if required.

No BUCK asset depends on functionality contained within any other BUCK asset, and while code and components have been provided which work with TextMeshPro and the new Unity Input System, these are optional and are no required within your project if you do not wish to use them. Furthermore, minor additional functionality does exist in some assets when another BUCK asset exists within the project's Package system, but this is again completely optional.

## Licensing
[LICENCE](License.md)