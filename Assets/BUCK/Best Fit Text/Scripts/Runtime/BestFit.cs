#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using UnityEngine;

namespace BUCK.BestFitText
{
    /// <inheritdoc />
    /// <summary>
    /// Class for detecting screen resolution or canvas size changes and assigning the minimum and maximum font size available for child Text components.
    /// </summary>
    [AddComponentMenu("Layout/Best Fit"), DisallowMultipleComponent]
    public class BestFit : MonoBehaviour
    {
        /// <summary>
        /// Event triggered by resolution width or height or camera rect changing.
        /// </summary>
        public static event Action ResolutionChange = delegate { };
        /// <summary>
        /// Last recorded screen resolution.
        /// </summary>
        protected static Vector2 _previousResolution = Vector2.zero;
        /// <summary>
        /// Camera used by parent Canvas.
        /// </summary>
        protected Camera _parentCamera;
        /// <summary>
        /// Last recorded camera rect.
        /// </summary>
        protected Rect _previousCamRect;
        /// <summary>
        /// The smallest allowed font size.
        /// </summary>
        [Tooltip("The smallest allowed font size.")]
        public int MinFontSize = 1;
        /// <summary>
        /// The largest allowed font size text.
        /// </summary>
        [Tooltip("The largest allowed font size text.")]
        public int MaxFontSize = 300;

        protected virtual void Awake()
        {
            if (_previousResolution == Vector2.zero)
            {
                _previousResolution = new Vector2(Screen.width, Screen.height);
            }
            _parentCamera = GetComponentInParent<Canvas>()?.worldCamera;
            if (_parentCamera != null)
            {
                _previousCamRect = _parentCamera.rect;
            }
        }

        protected virtual void LateUpdate()
        {
            if (!Mathf.Approximately(_previousResolution.x, Screen.width) || !Mathf.Approximately(_previousResolution.y, Screen.height))
            {
                _previousResolution = new Vector2(Screen.width, Screen.height);
                OnResolutionChange();
            }
            else if (_parentCamera && _previousCamRect != _parentCamera.rect)
            {
                _previousCamRect = _parentCamera.rect;
                OnResolutionChange();
            }
        }

        /// <summary>
        /// Fire the ResolutionChange event.
        /// </summary>
        public static void OnResolutionChange()
        {
            ResolutionChange?.Invoke();
        }
    }
}