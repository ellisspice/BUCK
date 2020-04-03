# Camera Controllers
## Description
A set of components related to limiting the aspect ratio or width of an orthographic Camera.

### SetCameraAspect
The SetCameraAspect Component is used to set a camera's aspect ratio to fit within the defined minimum and maximum.

### SetPortraitCamera
The SetPortraitCamera Component goes one step further, also ensuring that the camera aspect ratio is always portrait (height greater than or equal to width), which can be used to simulate a mobile experience on landscape devices.

### SetCameraWidth
The SetCameraWidth Component, which can be used in conjunction with the previously mentioned scripts, is used to ensure that the camera displays the defined width (in world units) at all times.

### BaseSetCamera
BaseSetCamera is an abstract class containing the basic functionality for detecting when the screen space has changed and an abstract method to call to set the camera values whenever any screen space changes occur. All of the provided Components inherit from this abstract class.

## Example Usage
To use any of the provided Components, add the Component to a GameObject containing the Camera Component (one will be added regardless as it is a required Component) and set the intended values for aspect ratio and/or width.