#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using UnityEngine;

namespace BUCK.AudioManagement.SFX
{
    /// <inheritdoc />
    /// <summary>
    /// Class for managing global sound effects.
    /// </summary>
    [AddComponentMenu("Audio/SFX Manager")]
    public class SFXManager : BaseSFXController<SFXClip>
    {
        /// <summary>
        /// Singleton.
        /// </summary>
        protected static SFXManager _instance;

        protected override void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this);
            base.Awake();
        }

        /// <summary>
        /// Play the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to play.</param>
        /// <param name="pitch">Optional. Pitch which the sound effect will be played.</param>
        public static void PlaySound(string effectName, float pitch = 1f)
        {
            _instance.Play(effectName, pitch);
        }

        /// <summary>
        /// Play the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to play.</param>
        /// <param name="startVolume">Starting volume of the sound effect. End volume also must be set for this value to be used.</param>
        /// <param name="endVolume">Volume of the sound effect at the end of playing.</param>
        /// <param name="pitch">Optional. Pitch which the sound effect will be played.</param>
        public static void PlaySoundAtVolume(string effectName, float startVolume, float endVolume, float pitch = 1f)
        {
            _instance.Play(effectName, pitch, startVolume, endVolume);
        }

        /// <summary>
        /// Stop playing (if playing) the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to stop.</param>
        public static void StopSound(string effectName)
        {
            _instance.Stop(effectName);
        }

        /// <summary>
        /// Stop all playing sound effects.
        /// </summary>
        public static void StopAllSounds()
        {
            _instance.StopAll();
        }

        /// <summary>
        /// Pause (if playing) the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to pause.</param>
        public static void PauseSound(string effectName)
        {
            _instance.Pause(effectName);
        }

        /// <summary>
        /// Pause all playing sound effects.
        /// </summary>
        public static void PauseAllSounds()
        {
            _instance.PauseAll();
        }

        /// <summary>
        /// Resume playing (if started playing) the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to resume.</param>
        public static void ResumeSound(string effectName)
        {
            _instance.Resume(effectName);
        }

        /// <summary>
        /// Resume all sound effects that have started playing.
        /// </summary>
        public static void ResumeAllSounds()
        {
            _instance.ResumeAll();
        }

        /// <summary>
        /// Is the sound effect with the provided name currently playing?
        /// </summary>
        /// <param name="effectName">Name of the clip to check status of.</param>
        /// <returns>Returns the playing status of the sound effect with the provided name.</returns>  
        public static bool IsPlayingSound(string effectName)
        {
            return _instance.IsPlaying(effectName);
        }
    }
}