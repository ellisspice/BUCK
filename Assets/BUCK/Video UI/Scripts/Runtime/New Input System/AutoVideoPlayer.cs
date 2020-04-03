#if NEW_INPUT_SYSTEM_INCLUDED
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace BUCK.VideoUI.NewInputSystem
{
    /// <inheritdoc />
    [AddComponentMenu("Video/Auto Video Player (New Input System)")]
    public class AutoVideoPlayer : BaseAutoVideoPlayer<InputSystemUIInputModule>
    {
        /// <inheritdoc />
        protected override bool CheckForInput()
        {
            return InputSystem.devices.Any(g => g.wasUpdatedThisFrame);
        }

        /// <inheritdoc />
        protected override void UpdateMousePosition()
        {
            _lastMousePosition = Pointer.current.position.ReadValue();
        }
    }
}
#endif