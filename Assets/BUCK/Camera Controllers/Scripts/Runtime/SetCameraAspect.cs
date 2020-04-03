using UnityEngine;

namespace BUCK.CameraControllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class which sets the camera resolution to fit within the defined minimum and maximum aspect ratio.
    /// </summary>
    [AddComponentMenu("Rendering/Camera Aspect Ratio Limiter"), DisallowMultipleComponent]
    public class SetCameraAspect : BaseSetCamera
    {
        /// <summary>
        /// The minimum aspect ratio the camera can be set to.
        /// </summary>
        [SerializeField, Tooltip("The minimum aspect ratio the camera can be set to.")]
        protected Vector2 _targetAspectMin = new Vector2(16, 9);
        /// <summary>
        /// The maximum aspect ratio the camera can be set to.
        /// </summary>
        [SerializeField, Tooltip("The maximum aspect ratio the camera can be set to.")]
        protected Vector2 _targetAspectMax = new Vector2(16, 9);

        /// <inheritdoc />
        /// <summary>
        /// Update the camera rect to fit within the defined allowable aspect ratios.
        /// </summary>
        protected override void UpdateCamera()
        {
            var aspect = (float)Screen.width / Screen.height;
            if (aspect > _targetAspectMax.x / _targetAspectMax.y)
            {
                var reduce = (aspect - (_targetAspectMax.x / _targetAspectMax.y)) / aspect;
                _cam.rect = new Rect(reduce * 0.5f, 0, 1 - reduce, 1);
            }
            else if (aspect < _targetAspectMin.x / _targetAspectMin.y)
            {
                var reduce = ((_targetAspectMin.x / _targetAspectMin.y) - aspect) / (_targetAspectMin.x / _targetAspectMin.y);
                _cam.rect = new Rect(0, reduce * 0.5f, 1, 1 - reduce);
            }
            else
            {
                _cam.rect = new Rect(0, 0, 1, 1);
            }
            SetCameraValues();
        }

        /// <summary>
        /// Set the camera aspect to match the aspect ratio currently set by the rect and record the current camera rect.
        /// </summary>
        protected virtual void SetCameraValues()
        {
            var rect = _cam.rect;
            _cam.aspect = (Screen.width * rect.width) / (Screen.height * rect.height);
            _previousCamRect = _cam.rect;
        }
    }
}