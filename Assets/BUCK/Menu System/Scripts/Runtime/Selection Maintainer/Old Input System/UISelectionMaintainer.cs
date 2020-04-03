using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace  BUCK.MenuSystem.SelectionMaintainer.OldInputSystem
{
    /// <inheritdoc />
    [AddComponentMenu("UI/UI Selection Maintainer")]
    public class UISelectionMaintainer : BaseUISelectionMaintainer<StandaloneInputModule>
    {
        /// <summary>
        /// Only axis inputs greater than this will be used. Should match value used in InputManager.
        /// </summary>
        protected const float InputDeadZone = 0.2f;

        /// <inheritdoc />
        protected override void GetInputStartState()
        {
            ControllerInput = Input.GetJoystickNames().Any(n => !string.IsNullOrEmpty(n));
        }

        /// <inheritdoc />
        protected override void GetCurrentInputType()
        {
            //Set to controller input if on console, mouse is not allowed or an input has come in from input axis or buttons.
            if (!ControllerInput && (Application.isConsolePlatform || !_allowMouseInput || Mathf.Abs(Input.GetAxis(_input.horizontalAxis)) >= InputDeadZone || Mathf.Abs(Input.GetAxis(_input.verticalAxis)) >= InputDeadZone || Input.GetButtonDown(_input.submitButton) || Input.GetButtonDown(_input.cancelButton)))
            {
                SetControllerInput(true);
            }
            //Set to not controller if not on console, mouse input is allowed and mouse position has changed or button has been clicked.
            else if (ControllerInput && !Application.isConsolePlatform && _allowMouseInput && (Input.touchCount > 0 || (Input.mousePresent && _lastMousePosition != Input.mousePosition) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                SetControllerInput(false);
            }
        }

        /// <inheritdoc />
        protected override void SetLastMousePosition()
        {
            if (ControllerInput)
            {
                _lastMousePosition = Input.mousePosition;
            }
        }

        /// <inheritdoc />
        protected override string FindCurrentDirection()
        {
            if (Mathf.Abs(Input.GetAxis(_input.horizontalAxis)) > Mathf.Abs(Input.GetAxis(_input.verticalAxis)))
            {
                if (Mathf.Abs(Input.GetAxis(_input.horizontalAxis)) >= InputDeadZone)
                {
                    return Input.GetAxis(_input.horizontalAxis) < 0 ? "left" : "right";
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxis(_input.verticalAxis)) >= InputDeadZone)
                {
                    return Input.GetAxis(_input.verticalAxis) < 0 ? "down" : "up";
                }
            }
            return string.Empty;
        }
    }
}