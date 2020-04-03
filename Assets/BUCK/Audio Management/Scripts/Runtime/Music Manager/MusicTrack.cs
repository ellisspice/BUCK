#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using UnityEngine;

namespace BUCK.AudioManagement.Music
{
    /// <summary>
    /// Object containing information related to a piece of music audio.
    /// </summary>
    [Serializable]
    public class MusicTrack
    {
        /// <summary>
        /// AudioClip to play.
        /// </summary>
        [Tooltip("AudioClip to play.")]
        public AudioClip Clip;
        /// <summary>
        /// Artist name.
        /// </summary>
        [Tooltip("Artist name.")]
        public string Artist;
        /// <summary>
        /// Track name.
        /// </summary>
        [Tooltip("Track name.")]
        public string TrackName;
        /// <summary>
        /// Base volume level.
        /// </summary>
        [Range(0f, 1f), Tooltip("Base volume level.")]
        public float Volume = 0.5f;
        /// <summary>
        /// AudioSource priority.
        /// Remember that a lower value mean it's a higher priority!
        /// </summary>
        [Range(0, 256), Tooltip("AudioSource priority. Remember that a lower value mean it's a higher priority!")]
        public int Priority = 128;
    }
}