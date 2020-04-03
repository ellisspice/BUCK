#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using UnityEngine;

namespace BUCK.AudioManagement.SFX
{
    /// <summary>
    /// Object containing information related to a piece of sound effect audio.
    /// </summary>
    [Serializable]
    public class SFXClip
    {
        /// <summary>
        /// Clip identifier.
        /// </summary>
        [Tooltip("Clip identifier.")]
        public string Name;
        /// <summary>
        /// AudioClip to play.
        /// </summary>
        [Tooltip("AudioClip to play.")]
        public AudioClip Clip;
        /// <summary>
        /// Volume level at the beginning of playing.
        /// </summary>
        [Range(0f, 1f), Tooltip("Volume level at the beginning of playing.")]
        public float StartVolume = 0.5f;
        /// <summary>
        /// Volume level at the end of playing.
        /// </summary>
        [Range(0f, 1f), Tooltip("Volume level at the end of playing.")]
        public float EndVolume = 0.5f;
        /// <summary>
        /// AudioSource priority. Remember that a lower value mean it's a higher priority!
        /// </summary>
        [Range(0, 256), Tooltip("AudioSource priority. Remember that a lower value mean it's a higher priority!")]
        public int Priority = 128;
        /// <summary>
        /// Should this clip being automatically played from the start?
        /// </summary>
        [Tooltip("Should this clip being automatically played from the start?")]
        public bool AutoPlay;
        /// <summary>
        /// Should this clip be looped?
        /// </summary>
        [Tooltip("Should this clip be looped?")]
        public bool Loop;
    }
}
