# Best Fit Text
## Description
Utilizes the built-in Text Component "Best Fit" feature to make a set of Text Components a uniform size based on the size of the smallest involved in that calculation.

## Example Usage
Example usage in a variety of scenarios can be found within the BestFitExample scene and the BestFitExample.cs script provided, with an example provide for both Unity UI and TextMeshPro.

BestFit can be called in the following ways:

**Resize all child Text Components of an object**

```csharp
GameObject.BestFit(bool includeInactive = true, List<string> newStrings = null);
Component.BestFit(bool includeInactive = true, List<string> newStrings = null);
```

**Resize all child Text Components of a collection of objects**

```csharp
List<Component>.BestFit(bool includeInactive = true, List<string> newStrings = null);
IEnumerable<Component>.BestFit(bool includeInactive = true, List<string> newStrings = null);
Component[].BestFit(bool includeInactive = true, List<string> newStrings = null);
List<GameObject>.BestFit(bool includeInactive = true, List<string> newStrings = null);
IEnumerable<GameObject>.BestFit(bool includeInactive = true, List<string> newStrings = null);
GameObject[].BestFit(bool includeInactive = true, List<string> newStrings = null);
```

- `includeInactive` determines if inactive GameObjects and disabled Text Components should be resized and used in resizing calculations.
- `newStrings` is an optional list of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.

The `ResolutionChange` event within BestFit is called whenever the screen resolution is changed or when the rect size of the Camera the parent Canvas of the BestFit Component is using is changed (if that Canvas is not using Screen Space Overlay Render Mode and a camera has been provided). Subscribing to this event means that the likely necessary resizing of text after such changes can be made automatically. An example of how to subscribe:

```csharp
private void OnEnable()
{
    BestFit.ResolutionChange += BestFitText;
}

private void OnDisable()
{
    BestFit.ResolutionChange += BestFitText;
}

private void BestFitText()
{
    //best fit text
}
```

In order for the `ResolutionChange` event to be fired one BestFit Component must exist within the scene. As such it is **heavily** recommended that a BestFit Component is added to every Canvas GameObject in a scene.

The `ResolutionChange` event also be fired by calling the following:

```csharp
void BestFit.OnResolutionChange();
```

### BestFit
This Component can be placed on any UI GameObject and sets the Minimum and Maximum possible font size for all child GameObjects for which this is the nearest parent BestFit Component. If a GameObject has no parent BestFit Component, a default Minimum font size of 1 and Maximum font size of 300 (1000 for TextMeshPro Components) are used in calculations.

### BestFitAutomatic
BestFitAutomatic inherits from BestFit and as such has all the same functionality. In addition, rather than requiring a `BestFit()` call in code, BestFitAutomatic will resize all child Text Components when the GameObject this is attached to is made active or reactive and when the screen or camera resolution is changed. Additionally, a resize can be manually called using the following:

```csharp
void OnChange();
void OnChange(bool includeInactive, List<string> newStrings = null);
```

## Potential Issues
- Inaccurate results will be displayed if GameObjects using UI Layout Components are made active after calling BestFit.
- Using BestFit with UI Layout Components can sometimes produce inaccurate results. One solution to this is to delay calling BestFit by a number of frames or recalling BestFit over a number of frames after the resolution change or object activation.
- It is recommended to call BestFit in both `OnEnable()` and `Start()`, as seen in both the BestFitAutomatic script and the BestFitExample script. Only calling in `OnEnable()` can result in inaccurate results when using Screen Space Camera canvases.