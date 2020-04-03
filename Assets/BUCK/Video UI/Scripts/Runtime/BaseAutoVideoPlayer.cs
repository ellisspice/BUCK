using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BUCK.VideoUI
{
    /// <inheritdoc />
    /// <summary>
    /// VideoPlayer UI which fades in and starts playing after a period of input inactivity.
    /// </summary>
    public abstract class BaseAutoVideoPlayer<T> : BaseVideoPlayer<Video> where T : BaseInputModule
    {
        /// <summary>
        /// Amount of seconds of inactivity before the video starts and fades in.
        /// </summary>
        [SerializeField, Tooltip("Amount of seconds of inactivity before the video starts and fades in.")]
        protected float _demoInactivityTime = 30;
        /// <summary>
        /// Amount of seconds it should take the video to fade in and out.
        /// </summary>
        [SerializeField, Tooltip("Amount of seconds it should take the video to fade in and out.")]
        protected float _timeToFade = 1;
        /// <summary>
        /// Time since last input.
        /// </summary>
        protected float _timeSinceLastAction;
        /// <summary>
        /// Last recorded position of the mouse.
        /// </summary>
        protected Vector3 _lastMousePosition;
        /// <summary>
        /// BaseInputModule within the current scene.
        /// </summary>
        protected T _input;
        /// <summary>
        /// CanvasGroup used to fade in and out.
        /// </summary>
        protected CanvasGroup _canvasGroup;

        protected override void OnEnable()
        {
            //Force PlayOnAwake to be false and Loop to be true.
            _playOnAwake = false;
            _loop = true;
            UpdateMousePosition();
            base.OnEnable();
        }

        protected virtual void Update()
        {
            //Check for any input and reset the timer if any are picked up.
            if (CheckForInput())
            {
                //Start fading the video out if there's input and it's currently playing.
                if (_timeSinceLastAction >= _demoInactivityTime)
                {
                    StartCoroutine(FadeOut());
                }
                _timeSinceLastAction = 0;
                UpdateMousePosition();
            }
            else
            {
                _timeSinceLastAction += Time.smoothDeltaTime;
            }
            if (_canvasGroup.alpha <= 0 && _timeSinceLastAction >= _demoInactivityTime)
            {
                StartCoroutine(FadeIn());
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Get or add the various components required to have a fully working video player UI.
        /// </summary>
        public override void GetVideoPlayer()
        {
            base.GetVideoPlayer();
            //Get or add the CanvasGroup.
            if (!_canvasGroup)
            {
                if (!TryGetComponent(out _canvasGroup))
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
            _input = FindObjectOfType<T>();
        }

        /// <summary>
        /// Check if any input is currently being made.
        /// </summary>
        /// <returns>If any input has been recorded this frame.</returns>
        protected abstract bool CheckForInput();

        /// <summary>
        /// Get and store the current position of the mouse or the last touch input.
        /// </summary>
        protected abstract void UpdateMousePosition();

        /// <summary>
        /// Start the video and increase the video alpha up to 1.
        /// </summary>
        protected virtual IEnumerator FadeIn()
        {
            PlayWithCurrentSettings();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            while (_timeSinceLastAction > _demoInactivityTime && _canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.smoothDeltaTime / (1 / _timeToFade);
                yield return _waitForEndOfFrame;
            }
        }

        /// <summary>
        /// Decrease the video alpha up to 0 and stop the video.
        /// </summary>
        protected virtual IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.smoothDeltaTime / (1 / _timeToFade);
                yield return _waitForEndOfFrame;
            }
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            Stop();
        }
    }
}