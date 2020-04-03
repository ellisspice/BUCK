using System;
using UnityEngine;
using UnityEngine.Video;

namespace BUCK.VideoUI
{
    /// <summary>
    /// Class used to set a video source and the video clip or url.
    /// </summary>
    [Serializable]
    public class Video
    {
        /// <summary>
        /// The source that the VideoPlayer uses for playback.
        /// </summary>
        [Tooltip("The source that the VideoPlayer uses for playback.")]
        public VideoSource VideoSource;
        /// <summary>
        /// The clip to be played by the VideoPlayer.
        /// </summary>
        [Tooltip("The clip to be played by the VideoPlayer.")]
        public VideoClip VideoClip;
        /// <summary>
        /// The file or HTTP URL that the VideoPlayer reads content from.
        /// </summary>
        [Tooltip("The file or HTTP URL that the VideoPlayer reads content from.")]
        public string VideoURL;
    }
}