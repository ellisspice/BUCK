#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BUCK.VideoUI
{
    /// <inheritdoc />
    /// <summary>
    /// Generic class for setting up a VideoPlayer UI.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BaseVideoPlayer<T> : MonoBehaviour where T : Video
    {
        /// <summary>
        /// Minimum playback speed.
        /// </summary>
        protected const float _minPlaybackSpeed = 0.25f;
        /// <summary>
        /// Maximum playback speed. While Unity allows playback speed to go up to 10 issues are often observed when going above 5.
        /// </summary>
        protected const float _maxPlaybackSpeed = 5;
        /// <summary>
        /// WaitForEndOfFrame used for Coroutines.
        /// </summary>
        protected readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        /// <summary>
        /// Video to play.
        /// </summary>
        [SerializeField, Tooltip("Video to play.")]
        protected T _video;
        /// <summary>
        /// Whether the content will start playing back as soon as the component awakes.
        /// </summary>
        [SerializeField, Tooltip("Whether the content will start playing back as soon as the component awakes.")]
        protected bool _playOnAwake = true;
        /// <summary>
        /// Determines whether the VideoPlayer will wait for the first frame to be loaded into the texture before starting playback when playOnAwake is on.
        /// </summary>
        [SerializeField, Tooltip("Determines whether the VideoPlayer will wait for the first frame to be loaded into the texture before starting playback when playOnAwake is on.")]
        protected bool _waitForFirstFrame = true;
        /// <summary>
        /// Determines whether the VideoPlayer restarts from the beginning when it reaches the end of the clip.
        /// </summary>
        [SerializeField, Tooltip("Determines whether the VideoPlayer restarts from the beginning when it reaches the end of the clip.")]
        protected bool _loop;
        /// <summary>
        /// Whether the VideoPlayer is allowed to skip frames to catch up with current time.
        /// </summary>
        [SerializeField, Tooltip("Whether the VideoPlayer is allowed to skip frames to catch up with current time.")]
        protected bool _skipOnDrop = true;
        /// <summary>
        /// Factor by which the basic playback rate will be multiplied.
        /// </summary>
        [SerializeField, Range(_minPlaybackSpeed, _maxPlaybackSpeed), Tooltip("Factor by which the basic playback rate will be multiplied.")]
        protected float _playbackSpeed = 1;
        /// <summary>
        /// Starting volume of the AudioSource used for video audio playback.
        /// </summary>
        [SerializeField, Range(0f, 1f), Tooltip("Starting volume of the AudioSource used for video audio playback.")]
        protected float _volume = 1;
        /// <summary>
        /// Should the AudioSource start muted?
        /// </summary>
        [SerializeField, Tooltip("Should the AudioSource start muted?")]
        protected bool _mute;
        
        /// <summary>
        /// VideoPlayer used for video playback.
        /// </summary>
        protected VideoPlayer _player;
        /// <summary>
        /// RawImage used to display the video.
        /// </summary>
        protected RawImage _image;
        /// <summary>
        /// AspectRatioFitter used to ensure the aspect ratio of the RawImage matches the aspect ratio of the video.
        /// </summary>
        protected AspectRatioFitter _aspectFitter;
        /// <summary>
        /// AudioSource used for playing video audio.
        /// </summary>
        protected AudioSource _audio;
        
        protected virtual void OnEnable()
        {
            GetVideoPlayer();
            _player.loopPointReached += VideoEnd;
            if (_player.playOnAwake)
            {
                Stop();
                PlayWithCurrentSettings();
            }
        }
        
        protected virtual void OnDisable()
        {
            _player.loopPointReached -= VideoEnd;
            Stop();
        }
        
        /// <summary>
        /// Get or add the various components required to have a fully working video player UI.
        /// </summary>
        public virtual void GetVideoPlayer()
        {
            //Get or add the AspectRatioFitter.
            if (!_aspectFitter)
            {
                if (!TryGetComponent(out _aspectFitter))
                {
                    _aspectFitter = gameObject.AddComponent<AspectRatioFitter>();
                }
                //Set the aspect ratio to the current (will look jarring before playing the first video otherwise) and the aspect mode to fit into the parent object.
                _aspectFitter.aspectRatio = (float)Screen.width / Screen.height;
                _aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            }
            //Get or add the VideoPlayer.
            if (!_player)
            {
                if (!TryGetComponent(out _player))
                {
                    _player = gameObject.AddComponent<VideoPlayer>();
                }
                _player.renderMode = VideoRenderMode.APIOnly;
                _player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            }
            //Get or add the RawImage.
            if (!_image)
            {
                if (!TryGetComponent(out _image))
                {
                    _image = gameObject.AddComponent<RawImage>();
                }
                _image.texture = null;
            }
            //Get or add the AudioSource.
            if (!_audio)
            {
                if (!TryGetComponent(out _audio))
                {
                    _audio = gameObject.AddComponent<AudioSource>();
                }
                _audio.playOnAwake = false;
                _player.controlledAudioTrackCount = 1;
                _player.SetTargetAudioSource(0, _audio);
                _player.EnableAudioTrack(0, true);
                SetSettings(_loop, _playbackSpeed, _volume, _mute, _skipOnDrop, _playOnAwake, _waitForFirstFrame);
            }
        }

        /// <summary>
        /// Update the RawImage AspectRatio to match the aspect ratio of the current video.
        /// </summary>
        protected virtual void UpdateImageSize()
        {
            if (_player.source == VideoSource.VideoClip)
            {
                var clip = _player.clip;
                _aspectFitter.aspectRatio = (float)clip.width / clip.height;
            }
            else
            {
                var texture = _player.texture;
                _aspectFitter.aspectRatio = (float)texture.width / texture.height;
            }
        }
        
        /// <summary>
        /// Set the video to be played.
        /// </summary>
        /// <param name="video">Video object to set what to play.</param>
        public virtual void SetVideo(T video)
        {
            GetVideoPlayer();
            _video = video;
        }
        
        /// <summary>
        /// Play the current video using the settings defined on this component.
        /// </summary>
        public virtual void PlayWithPredefinedSettings()
        {
            Play(_loop, _playbackSpeed, _volume, _mute, _skipOnDrop, _playOnAwake, _waitForFirstFrame);
        }
        
        /// <summary>
        /// Play the current video using the settings currently defined on the VideoPlayer and AudioSource components.
        /// </summary>
        public virtual void PlayWithCurrentSettings()
        {
            Play(_player.isLooping, _player.playbackSpeed, _audio.volume, _audio.mute, _player.skipOnDrop, _player.playOnAwake, _player.waitForFirstFrame);
        }

        /// <summary>
        /// Play the current video using the settings provided.
        /// </summary>
        /// <param name="loop">Optional. Should the loop play again once it finishes?</param>
        /// <param name="playbackSpeed">Optional. The speed of playback.</param>
        /// <param name="volume">Optional. The volume of the video audio.</param>
        /// <param name="mute">Optional. Should the audio be muted?</param>
        /// <param name="skipOnDrop">Optional. Is the video allowed to skip frames in order to catch up to current time?</param>
        /// <param name="playOnAwake">Optional. Should the video start playing once the component is enabled?</param>
        /// <param name="waitForFirstFrame">Optional. Should the VideoPlayer wait for the first frame to be loaded into the texture before starting playback if playOnAwake is true?</param>
        public virtual void Play(bool loop = false, float playbackSpeed = 1f, float volume = 1f, bool mute = false, bool skipOnDrop = true, bool playOnAwake = true, bool waitForFirstFrame = true)
        {
            SetSettings(loop, playbackSpeed, volume, mute, skipOnDrop, playOnAwake, waitForFirstFrame);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(PlayVideo());
            }
        }
        
        /// <summary>
        /// Play the current video.
        /// </summary>
        protected virtual IEnumerator PlayVideo()
        {
            //Stop in case it is already playing.
            Stop();
            //Set the source and url/clip to match the current video.
            _player.source = _video.VideoSource;
            if (_video.VideoSource == VideoSource.VideoClip)
            {
                _player.url = string.Empty;
                _player.clip = _video.VideoClip;
            }
            else
            {
                _player.clip = null;
                _player.url = _video.VideoURL;
            }
            //Prepare the video and wait while it is preparing.
            _player.Prepare();
            while (!_player.isPrepared)
            {
                yield return _waitForEndOfFrame;
            }
            //Update the RawImage AspectRatio to match the video.
            UpdateImageSize();
            //Set the Image texture to match the VideoPlayer texture.
            _image.texture = _player.texture;
            //Start playing the video.
            _player.Play();
        }

        /// <summary>
        /// Method triggered by the video ending.
        /// </summary>
        /// <param name="source">The VideoPlayer in which the video finished.</param>
        protected virtual void VideoEnd(VideoPlayer source)
        {
            //If the VideoPlayer is not set to loop, trigger the Stop method.
            if (!source.isLooping)
            {
                Stop();
            }
            //Otherwise, trigger the PlayWithCurrentSettings method.
            else
            {
                PlayWithCurrentSettings();
            }
        }

        /// <summary>
        /// Continue playing a video.
        /// </summary>
        public virtual void Continue()
        {
            GetVideoPlayer();
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            //If the video has not been started rather than paused, trigger PlayWithCurrentSettings instead.
            if (Mathf.Approximately((float)_player.time, 0))
            {
                PlayWithCurrentSettings();
                return;
            }
            _player.Play();
        }
        
        /// <summary>
        /// Pause the video.
        /// </summary>
        public virtual void Pause()
        {
            GetVideoPlayer();
            _player.Pause();
        }

        /// <summary>
        /// Stop the video.
        /// </summary>
        public virtual void Stop()
        {
            _image.texture = null;
            _player.Stop();
            _player.clip = null;
            _player.url = string.Empty;
            _player.time = 0;
        }

        /// <summary>
        /// Update the settings for the VideoPlayer and AudioSource.
        /// </summary>
        /// <param name="loop">Should the loop play again once it finishes?</param>
        /// <param name="playbackSpeed">The speed of playback.</param>
        /// <param name="volume">The volume of the video audio.</param>
        /// <param name="mute">Should the audio be muted?</param>
        /// <param name="skipOnDrop">Is the video allowed to skip frames in order to catch up to current time?</param>
        /// <param name="playOnAwake">Should the video start playing once the component is enabled?</param>
        /// <param name="waitForFirstFrame">Should the VideoPlayer wait for the first frame to be loaded into the texture before starting playback if playOnAwake is true?</param>
        public virtual void SetSettings(bool loop, float playbackSpeed, float volume, bool mute, bool skipOnDrop, bool playOnAwake, bool waitForFirstFrame)
        {
            _player.playOnAwake = playOnAwake;
            _player.isLooping = loop;
            _player.waitForFirstFrame = waitForFirstFrame;
            _player.skipOnDrop = skipOnDrop;
            _audio.volume = volume;
            _audio.mute = mute;
            _player.playbackSpeed = Mathf.Clamp(playbackSpeed, _minPlaybackSpeed, _maxPlaybackSpeed);
        }

        /// <summary>
        /// Set the AudioSource to be muted.
        /// </summary>
        public virtual void Mute()
        {
            _audio.mute = true;
        }
        
        /// <summary>
        /// Set the AudioSource to not be muted.
        /// </summary>
        public virtual void Unmute()
        {
            _audio.mute = false;
        }

        protected virtual void OnDestroy()
        {
            Stop();
        }
    }
}