using UnityEngine;

namespace BUCK.CameraControllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class which sets the camera orthographic size to match the defined width.
    /// </summary>
    [AddComponentMenu("Rendering/Camera Width Limiter"), DisallowMultipleComponent]
    public class SetCameraWidth : BaseSetCamera
    {
        /// <summary>
        /// The size in world units the camera width should be set to.
        /// </summary>
        [SerializeField, Tooltip("The size in world units the camera width should be set to.")]
        protected float _targetWidth;
        
        /// <inheritdoc />
        /// <summary>
        /// Update the camera orthographic size so that the camera width displays the target width.
        /// </summary>
        protected override void UpdateCamera()
        {
            _cam.orthographicSize = _targetWidth / _cam.aspect;
        }
        
        /// <summary>
        /// Set the target width that this camera should display.
        /// </summary>
        /// <param name="width">Value which the targetWidth should be set to.</param>
        protected virtual void SetTargetWidth(float width)
        {
            _targetWidth = width;
            UpdateCamera();
        }
    }
}