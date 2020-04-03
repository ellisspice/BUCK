using UnityEngine;

namespace BUCK.CameraControllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class which sets the camera resolution to always be a portrait aspect ratio and to fit within the defined minimum and maximum aspect ratio.
    /// </summary>
    [AddComponentMenu("Rendering/Camera Portrait Aspect Ratio Limiter"), DisallowMultipleComponent]
    public class SetPortraitCamera : SetCameraAspect
    {
        protected override void Awake()
        {
            //disable component if either target aspect ratio is not a portrait aspect ratio
            if (_targetAspectMin.x / _targetAspectMin.y > 1)
            {
                Debug.LogWarning("ForcePortraitCamera Component disabled as provided target minimum aspect ratio is landscape, not portrait");
                enabled = false;
            }
            else if (_targetAspectMax.x / _targetAspectMax.y > 1)
            {
                Debug.LogWarning("ForcePortraitCamera Component disabled as provided target maximum aspect ratio is landscape, not portrait");
                enabled = false;
            }
            else
            {
                base.Awake();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Update the camera rect to be a portrait aspect ratio if the screen is landscape, to fit within the defined allowable aspect ratios and to match the expected orthographic size.
        /// Landscape aspect ratios are by default flipped (e.g. 16 by 9 becomes 9 by 16) if they fit within the allowable aspect ratio range.
        /// </summary>
        protected override void UpdateCamera()
        {
            var aspect = (float)Screen.width / Screen.height;
            if (aspect > 1)
            {
                var portraitAspect = (float)Screen.height / Screen.width;
                var portrait = Mathf.Pow(portraitAspect, 2);
                if (portrait > Mathf.Pow(_targetAspectMax.x / _targetAspectMax.y, 2))
                {
                    portrait = portraitAspect * (_targetAspectMax.x / _targetAspectMax.y);
                }
                else if (portrait < Mathf.Pow(_targetAspectMin.x / _targetAspectMin.y, 2))
                {
                    portrait = portraitAspect * (_targetAspectMin.x / _targetAspectMin.y);
                }
                _cam.rect = new Rect(new Vector2((1 - portrait) / 2, 0), new Vector2(portrait, 1));
                SetCameraValues();
            }
            else
            {
                base.UpdateCamera();
            }
        }
    }
}