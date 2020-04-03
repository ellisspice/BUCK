using UnityEngine;

namespace BUCK.CameraControllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class for setting the camera aspect ratio or orthographic size.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class BaseSetCamera : MonoBehaviour
    {
        /// <summary>
        /// Last recorded screen resolution.
        /// </summary>
        protected Vector2 _previousResolution = Vector2.zero;
        /// <summary>
        /// Last recorded full screen setting.
        /// </summary>
        protected bool _previousFull;
        /// <summary>
        /// Camera that is being adjusted.
        /// </summary>
        protected Camera _cam;
        /// <summary>
        /// Last recorded rect of the camera.
        /// </summary>
        protected Rect _previousCamRect;
        
        protected virtual void Awake()
        { 
            TryGetComponent(out _cam);
            _previousCamRect = _cam.rect;
            _previousResolution = new Vector2(Screen.width, Screen.height);
            _previousFull = Screen.fullScreen;
            UpdateCamera();
        }
        
        protected virtual void LateUpdate()
        {
            if (!Mathf.Approximately(_previousResolution.x, Screen.width) || !Mathf.Approximately(_previousResolution.y, Screen.height) || _previousFull != Screen.fullScreen)
            {
                _previousResolution = new Vector2(Screen.width, Screen.height);
                _previousFull = Screen.fullScreen;
                UpdateCamera();
            }
            else if (_previousCamRect != _cam.rect)
            {
                _previousCamRect = _cam.rect;
                UpdateCamera();
            }
        }

        /// <summary>
        /// Update camera settings after a change of screen resolution or camera rect size.
        /// </summary>
        protected abstract void UpdateCamera();
    }
}