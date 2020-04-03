using UnityEngine;

namespace BUCK.AudioManagement
{
    /// <inheritdoc />
    /// <summary>
    /// Class for managing audio.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BaseAudioManager : MonoBehaviour
    {
        /// <summary>
        /// Key used to save and load overall audio volume from PlayerPrefs.
        /// </summary>
        protected const string VolumePrefsKey = "Volume";
        /// <summary>
        /// Maximum volume value.
        /// </summary>
        protected const int MaxVolume = 10;
        /// <summary>
        /// Overall application volume. Max volume value set in MaxVolume.
        /// </summary>
        protected static int _volume;
        /// <summary>
        /// Overall application volume. Max volume value set in MaxVolume.
        /// </summary>
        public static int Volume {  get => _volume; set => UpdateVolume(value); }

        protected virtual void Awake()
        {
            //Get last saved volume, default to MaxVolume if volume has not been set before.
            UpdateVolume(PlayerPrefs.GetInt(VolumePrefsKey, MaxVolume));
        }

        /// <summary>
        /// Update the overall volume of the application.
        /// </summary>
        /// <param name="amount">Set the volume value to this amount.</param>
        public static void UpdateVolume(int amount)
        {
            //Ensure volume cannot go below 0 and above MaxVolume.
            var newVolume = Mathf.Clamp(amount, 0, MaxVolume);
            _volume = newVolume;
            PlayerPrefs.SetInt(VolumePrefsKey, newVolume);
            AudioListener.volume = _volume * (1f/MaxVolume);
        }
    }
}