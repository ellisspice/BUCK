using UnityEngine;
using UnityEngine.EventSystems;

namespace BUCK.VideoUI.OldInputSystem
{
    /// <inheritdoc />
    [AddComponentMenu("Video/Auto Video Player")]
    public class AutoVideoPlayer : BaseAutoVideoPlayer<StandaloneInputModule>
    {
        /// <summary>
        /// Only axis inputs greater than this will be used. Should ideally match value used in InputManager.
        /// </summary>
        protected const float InputDeadZone = 0.2f;

        /// <inheritdoc />
        protected override bool CheckForInput()
        {
            return Mathf.Abs(Input.GetAxis(_input.horizontalAxis)) >= InputDeadZone ||
                   Mathf.Abs(Input.GetAxis(_input.verticalAxis)) >= InputDeadZone || Input.anyKeyDown ||
                   Input.touchCount > 0 || (Input.mousePresent && _lastMousePosition != Input.mousePosition);
        }

        /// <inheritdoc />
        protected override void UpdateMousePosition()
        {
            _lastMousePosition = Input.mousePosition;
        }
    }
}