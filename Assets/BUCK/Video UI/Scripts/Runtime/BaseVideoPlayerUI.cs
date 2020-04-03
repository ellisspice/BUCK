#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BUCK.VideoUI
{
    /// <inheritdoc />
    /// <summary>
    /// VideoPlayer UI with a set of inputs for controlling playback.
    /// </summary>
    public abstract class BaseVideoPlayerUI<T, U> : BaseVideoPlayer<Video> where T : MaskableGraphic where U : Selectable
    {
        /// <summary>
        /// Available playback speeds for the playback dropdown.
        /// </summary>
        protected readonly float[] _playbackSpeedOptions = {0.25f, 0.5f, 1, 2, 3, 5};
        /// <summary>
        /// Button used to trigger the video to play.
        /// </summary>
        [SerializeField, Tooltip("Button used to trigger the video to play.")]
        protected Button _playButton;
        /// <summary>
        /// Button used to trigger the video to pause.
        /// </summary>
        [SerializeField, Tooltip("Button used to trigger the video to pause.")]
        protected Button _pauseButton;
        /// <summary>
        /// Button used to trigger the video audio to be muted.
        /// </summary>
        [SerializeField, Tooltip("Button used to trigger the video audio to be muted.")]
        protected Button _muteButton;
        /// <summary>
        /// Button used to trigger the video audio to no longer be muted.
        /// </summary>
        [SerializeField, Tooltip("Button used to trigger the video audio to no longer be muted.")]
        protected Button _unmuteButton;
        /// <summary>
        /// Button used to set the video to play again once finished.
        /// </summary>
        [SerializeField, Tooltip("Button used to set the video to play again once finished.")]
        protected Button _loopButton;
        /// <summary>
        /// Button used to set the video to not play again once finished.
        /// </summary>
        [SerializeField, Tooltip("Button used to set the video to not play again once finished.")]
        protected Button _unloopButton;
        /// <summary>
        /// Slider which displays and can set the current position in video playback.
        /// </summary>
        [SerializeField, Tooltip("Slider which displays and can set the current position in video playback.")]
        protected Slider _positionSlider;
        /// <summary>
        /// Slider which displays and can set the current video audio volume.
        /// </summary>
        [SerializeField, Tooltip("Slider which displays and can set the current video audio volume.")]
        protected Slider _volumeSlider;
        /// <summary>
        /// Dropdown which displays and can set the playback speed.
        /// </summary>
        [SerializeField, Tooltip("Dropdown which displays and can set the playback speed.")]
        protected U _playbackDropdown;
        /// <summary>
        /// Text which displays the current time into the video and the total length of the video.
        /// </summary>
        [SerializeField, Tooltip("Text which displays the current time into the video and the total length of the video.")]
        protected T _timer;

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateUI(false);
        }

        /// <summary>
        /// Update the active and value settings for provided UI.
        /// </summary>
        /// <param name="playing">Should the UI be set up for a playing video?</param>
        protected virtual void UpdateUI(bool playing)
        {
            if (_playButton)
            {
                _playButton.gameObject.SetActive(!playing);
            }
            if (_pauseButton)
            {
                _pauseButton.gameObject.SetActive(playing);
            }
            if (_muteButton)
            {
                _muteButton.gameObject.SetActive(!_audio.mute);
            }
            if (_unmuteButton)
            {
                _unmuteButton.gameObject.SetActive(_audio.mute);
            }
            if (_loopButton)
            {
                _loopButton.gameObject.SetActive(!_player.isLooping);
            }
            if (_unloopButton)
            {
                _unloopButton.gameObject.SetActive(_player.isLooping);
            }
            if (_volumeSlider)
            {
                _volumeSlider.value = _audio.volume;
            }
        }

        /// <inheritdoc />
        protected override IEnumerator PlayVideo()
        {
            UpdateUI(false);
            if (_positionSlider)
            {
                _positionSlider.value = 0;
            }
            if (_volumeSlider)
            {
                _audio.volume = _volumeSlider.value;
            }
            if (_playbackDropdown)
            {
                UpdatePlaybackSpeed();
            }
            yield return base.PlayVideo();
            UpdateUI(true);
            var clipLength = _player.source == VideoSource.VideoClip ? _player.clip.length : _player.frameCount / _player.frameRate;
            var length = TimeSpan.FromSeconds(clipLength);
            while ((_player.source == VideoSource.VideoClip ? _player.clip : !string.IsNullOrEmpty(_player.url)) && clipLength > _player.time)
            {
                if (_positionSlider)
                {
                    //If the slider value is out of sync with the real value, update slider value.
                    if (Mathf.Abs((float)(_player.time - (_positionSlider.value * clipLength))) < Time.smoothDeltaTime * _player.playbackSpeed * 10)
                    {
                        //SetValueWithoutNotify used to avoid additional calls to UpdateVideoPosition.
                        _positionSlider.SetValueWithoutNotify((float)(_player.time / clipLength));
                    }
                }
                if (_timer)
                {
                    UpdateTimerText(length);
                }
                yield return _waitForEndOfFrame;
            }
        }

        /// <inheritdoc />
        public override void Continue()
        {
            base.Continue();
            UpdateUI(true);
        }

        /// <summary>
        /// Toggle between playing and pausing.
        /// </summary>
        public virtual void PlayToggle()
        {
            if (_player.isPlaying)
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }

        /// <inheritdoc />
        public override void Pause()
        {
            base.Pause();
            UpdateUI(false);
        }

        /// <inheritdoc />
        public override void Stop()
        {
            base.Stop();
            UpdateUI(false);
        }

        /// <summary>
        /// Update the current video position based on the new value of the video position slider.
        /// </summary>
        public virtual void UpdateVideoPosition()
        {
            var clipLength = _player.source == VideoSource.VideoClip ? _player.clip.length : _player.frameCount / _player.frameRate;
            if (!Mathf.Approximately((float) _player.time, (float) (_positionSlider.value * clipLength)))
            {
                _player.time = _positionSlider.value * clipLength;
            }
        }
        
        /// <summary>
        /// Update the current video audio volume to match the value of the slider.
        /// </summary>
        public virtual void UpdateVolume()
        {
            _audio.volume = _volumeSlider.value;
        }

        /// <summary>
        /// Update the current video playback speed based on the option selected in the dropdown.
        /// </summary>
        public abstract void UpdatePlaybackSpeed();

        /// <summary>
        /// Update the displayed timer text.
        /// </summary>
        /// <param name="length">Length of the currently playing video.</param>
        protected abstract void UpdateTimerText(TimeSpan length);

        /// <inheritdoc />
        public override void Mute()
        {
            base.Mute();
            UpdateUI(_player.isPlaying);
        }

        /// <inheritdoc />
        public override void Unmute()
        {
            base.Unmute();
            UpdateUI(_player.isPlaying);
        }
        
        /// <summary>
        /// Set the video to play again once it has finished playing.
        /// </summary>
        public virtual void Loop()
        {
            _player.isLooping = true;
            UpdateUI(_player.isPlaying);
        }
        
        /// <summary>
        /// Set the video to not play again once it has finished playing.
        /// </summary>
        public virtual void Unloop()
        {
            _player.isLooping = false;
            UpdateUI(_player.isPlaying);
        }
    }
}