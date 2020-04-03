using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace BUCK.VideoUI.UnityUI
{
    /// <inheritdoc />
    /// <summary>
    /// VideoPlayer UI for playing a set of videos, with a set of inputs for controlling playback.
    /// </summary>
    [AddComponentMenu("Video/Video Playlist Player UI")]
    public class VideoPlaylistPlayerUI : VideoPlayerUI
    {
        /// <summary>
        /// List of videos to be played.
        /// </summary>
        [SerializeField, Tooltip("List of videos to be played.")]
        protected List<Video> _videoPlaylist;
        /// <summary>
        /// Index of the current video being played.
        /// </summary>
        protected int _videoIndex;

        /// <inheritdoc />
        public override void Play(bool loop = false, float playbackSpeed = 1, float volume = 1, bool mute = false, bool skipOnDrop = true, bool playOnAwake = true, bool waitForFirstFrame = true)
        {
            //Video is not set if the list of videos is empty.
            if (_videoPlaylist.Count > 0)
            {
                SetVideo(_videoPlaylist[_videoIndex]);
                base.Play(loop, playbackSpeed, volume, mute, skipOnDrop, playOnAwake, waitForFirstFrame);
            }
        }

        /// <inheritdoc />
        protected override void VideoEnd(VideoPlayer source)
        {
            base.VideoEnd(source);
            //Start playing the next video if the video is not set to loop.
            if (!source.isLooping)
            {
                GoToNext();
            }
        }

        /// <summary>
        /// Skip back to the previous video in the playlist or the beginning of the current video, depending on the current video playback position.
        /// </summary>
        public virtual void GoToPrevious()
        {
            var clipLength = _player.source == VideoSource.VideoClip ? _player.clip.length : _player.frameCount / _player.frameRate;
            if (_player.time > Mathf.Min(5, (float)(clipLength * 0.25f)))
            {
                _videoIndex--;
                if (_videoIndex < 0)
                {
                    _videoIndex = _videoPlaylist.Count - 1;
                }
                PlayWithCurrentSettings();
            }
            else
            {
                _player.time = 0;
            }
        }
        
        /// <summary>
        /// Skip to the next video in the playlist.
        /// </summary>
        public virtual void GoToNext()
        {
            _videoIndex++;
            if (_videoIndex >= _videoPlaylist.Count)
            {
                _videoIndex = 0;
            }
            PlayWithCurrentSettings();
        }
    }
}