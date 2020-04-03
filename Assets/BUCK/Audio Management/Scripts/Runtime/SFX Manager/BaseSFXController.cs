#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections.Generic;
using UnityEngine;

namespace BUCK.AudioManagement.SFX
{
    /// <inheritdoc />
    /// <summary>
    /// Class for managing sound effects.
    /// </summary>
    public abstract class BaseSFXController : BaseAudioManager
    {
        /// <summary>
        /// Key used to save and load sound effect volume from PlayerPrefs.
        /// </summary>
        protected const string SFXVolumePrefsKey = "SFX Volume";
        /// <summary>
        /// Sound effect volume in application. Max volume is value set in MaxVolume.
        /// </summary>
        protected static int _sfxVolume;
    }
    
    /// <inheritdoc />
    public abstract class BaseSFXController<T> : BaseSFXController where T : SFXClip
    {
        /// <summary>
        /// List of sound effects to use.
        /// </summary>
        [SerializeField, Tooltip("List of sound effects to use.")]
        protected List<T> _audioClips = new List<T>();
        /// <summary>
        /// Dictionary of sound effects keys and their associated AudioSource.
        /// </summary>
        protected readonly Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();
        /// <summary>
        /// Dictionary of sound effects keys and their associated SfxClip.
        /// </summary>
        protected readonly Dictionary<string, T> _clipDict = new Dictionary<string, T>();
        /// <summary>
        /// Sound effect volume in application. Max volume is value set in MaxVolume.
        /// </summary>
        public static int SfxVolume { get => _sfxVolume; set => UpdateSfxVolume(value); }

        protected override void Awake()
        {
            base.Awake();
            //Get last saved volume, default to max volume if volume has not been set before.
            UpdateSfxVolume(PlayerPrefs.GetInt(SFXVolumePrefsKey, MaxVolume));
            //For each clip provided, set up an audio source.
            foreach (var audioClip in _audioClips)
            {
                if (!_audioSources.ContainsKey(audioClip.Name))
                {
                    SetUpAudioSource(audioClip);
                }
                else
                {
                    Debug.LogWarning("Clip with name " + audioClip.Name + " not set up due to duplicate clip name");
                }
            }
        }

        protected virtual void Update()
        {
            //For every playing AudioSource, update volume to match current volume for sound effects and volume expected at this point within the sound effect.
            foreach (var source in _audioSources)
            {
                if (source.Value.isPlaying)
                {
                    source.Value.volume = Mathf.Lerp(_clipDict[source.Key].StartVolume * _sfxVolume * (1f/MaxVolume), _clipDict[source.Key].EndVolume * _sfxVolume * (1f/MaxVolume), source.Value.time / source.Value.clip.length);
                }
            }
        }

        /// <summary>
        /// Set up an AudioSource for this clip.
        /// </summary>
        /// <param name="audioClip">The clip to set up.</param>
        protected virtual void SetUpAudioSource(T audioClip)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = audioClip.Clip;
            source.priority = audioClip.Priority;
            source.volume = audioClip.StartVolume;
            source.loop = audioClip.Loop;
            source.playOnAwake = audioClip.AutoPlay;
            if (audioClip.AutoPlay)
            {
                source.Play();
            }
            _audioSources.Add(audioClip.Name, source);
            _clipDict.Add(audioClip.Name, audioClip);
        }

        /// <summary>
        /// Play the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to play.</param>
        /// <param name="pitch">Optional. Pitch which the sound effect will be played.</param>
        /// <param name="startVolume">Optional. Starting volume of the sound effect. End volume also must be set for this value to be used.</param>
        /// <param name="endVolume">Optional. Volume of the sound effect at the end of playing.</param>
        public virtual void Play(string effectName, float pitch = 1f, float startVolume = -1f, float endVolume = -1f)
        {
            if (_audioSources.TryGetValue(effectName, out var audioSource))
            {
                if (startVolume > -1f && endVolume > -1f)
                {
                    audioSource.volume = startVolume;
                    _clipDict[effectName].StartVolume = startVolume;
                    _clipDict[effectName].EndVolume = endVolume;
                }
                audioSource.Stop();
                audioSource.pitch = pitch;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Invalid Clip Name: " + effectName);
            }
        }

        /// <summary>
        /// Stop playing (if playing) the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to stop.</param>
        public virtual void Stop(string effectName)
        {
            if (_audioSources.TryGetValue(effectName, out var audioSource))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
            else
            {
                Debug.LogWarning("Invalid Clip Name: " + effectName);
            }
        }

        /// <summary>
        /// Stop all playing sound effects.
        /// </summary>
        public virtual void StopAll()
        {
            foreach (var sound in _audioSources.Keys)
            {
                Stop(sound);
            }
        }

        /// <summary>
        /// Pause (if playing) the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to pause.</param>
        public virtual void Pause(string effectName)
        {
            if (_audioSources.TryGetValue(effectName, out var audioSource))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
            else
            {
                Debug.LogWarning("Invalid Clip Name: " + effectName);
            }
        }

        /// <summary>
        /// Pause all playing sound effects.
        /// </summary>
        public virtual void PauseAll()
        {
            foreach (var sound in _audioSources.Keys)
            {
                Pause(sound);
            }
        }

        /// <summary>
        /// Resume playing (if started playing) the sound effect with the provided name.
        /// </summary>
        /// <param name="effectName">Name of the clip to resume.</param>
        public virtual void Resume(string effectName)
        {
            if (_audioSources.TryGetValue(effectName, out var audioSource))
            {
                if (audioSource.time > 0)
                {
                    audioSource.Play();
                }
            }
            else
            {
                Debug.LogWarning("Invalid Clip Name: " + effectName);
            }
        }

        /// <summary>
        /// Resume all sound effects that have started playing.
        /// </summary>
        public virtual void ResumeAll()
        {
            foreach (var sound in _audioSources.Keys)
            {
                Resume(sound);
            }
        }

        /// <summary>
        /// Is the sound effect with the provided name currently playing?
        /// </summary>
        /// <param name="effectName">Name of the clip to check status of.</param>
        /// <returns>Return the playing status of the sound effect with the provided name.</returns>  
        public virtual bool IsPlaying(string effectName)
        {
            if (_audioSources.TryGetValue(effectName, out var audioSource))
            {
                return audioSource.isPlaying;
            }
            Debug.LogWarning("Invalid Clip Name: " + effectName);
            return false;
        }

        /// <summary>
        /// Update the sound effect volume of the application.
        /// </summary>
        /// <param name="amount">Set the volume value to this amount.</param>
        public static void UpdateSfxVolume(int amount)
        {
            var newVolume = Mathf.Clamp(amount, 0, MaxVolume);
            _sfxVolume = newVolume;
            PlayerPrefs.SetInt(SFXVolumePrefsKey, newVolume);
        }
    }
}