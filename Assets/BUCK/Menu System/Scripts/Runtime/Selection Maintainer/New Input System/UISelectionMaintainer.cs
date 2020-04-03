#if NEW_INPUT_SYSTEM_INCLUDED
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace BUCK.MenuSystem.SelectionMaintainer.NewInputSystem
{
    /// <inheritdoc />
    [AddComponentMenu("UI/UI Selection Maintainer (New Input System)")]
    public class UISelectionMaintainer : BaseUISelectionMaintainer<InputSystemUIInputModule>
    {
        /// <inheritdoc />
        protected override void GetInputStartState()
        {
            ControllerInput = Gamepad.all.Count > 0;
            _input.deselectOnBackgroundClick = false;
        }

        /// <inheritdoc />
        protected override void GetCurrentInputType()
        {
            //Set to controller input if on console, mouse is not allowed or an input has come in from input axis or buttons.
            if (!ControllerInput && (Application.isConsolePlatform || !_allowMouseInput || InputSystem.devices.Any(i => i.wasUpdatedThisFrame && (i.GetType() == typeof(Keyboard) || i.GetType() == typeof(Gamepad)))))
            {
                SetControllerInput(true);
            }
            //Set to not controller if not on console, mouse input is allowed and mouse position has changed or button has been clicked.
            else if (ControllerInput && !Application.isConsolePlatform && _allowMouseInput && InputSystem.devices.Any(i => i.wasUpdatedThisFrame && (i.GetType() == typeof(Mouse)) || i.GetType() == typeof(Touchscreen) || i.GetType() == typeof(Pen)))
            {
                SetControllerInput(false);
            }
        }

        /// <inheritdoc />
        protected override void SetLastMousePosition()
        {
            if (ControllerInput && Pointer.current != null)
            {
                _lastMousePosition = Pointer.current.position.ReadValue();
            }
        }

        /// <inheritdoc />
        protected override string FindCurrentDirection()
        {
            var input = _input.move.action.ReadValue<Vector2>();
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (Mathf.Abs(input.x) >= InputSystem.settings.defaultDeadzoneMin)
                {
                    return input.x < 0 ? "left" : "right";
                }
            }
            else
            {
                if (Mathf.Abs(input.y) >= InputSystem.settings.defaultDeadzoneMin)
                {
                    return input.y < 0 ? "down" : "up";
                }
            }
            return string.Empty;
        }
    }
}
#endif