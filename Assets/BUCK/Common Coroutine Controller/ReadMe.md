# Common Coroutine Controller
## Description
The CommonCoroutine is a singleton component designed to be called when Coroutine functionality is required within a non-MonoBehaviour script.

## Example Usage
```csharp
// Static method to start a Coroutine.
Coroutine CommonCoroutine.StartStaticCoroutine(IEnumerator routine);

// Static method to stop a currently running Coroutine for this IEnumerator.
void CommonCoroutine.StopStaticCoroutine(IEnumerator routine);

// Static method to stop the provided Coroutine.
void CommonCoroutine.StopStaticCoroutine(Coroutine routine);

// Static method to stop all Coroutines running within this Component.
void CommonCoroutine.StopAllStaticCoroutines()
```