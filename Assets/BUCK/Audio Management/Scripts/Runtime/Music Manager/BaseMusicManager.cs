#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BUCK.AudioManagement.Music
{
    /// <inheritdoc />
    /// <summary>
    /// Class for managing music.
    /// </summary>
    public abstract class BaseMusicManager : BaseAudioManager
    {
        /// <summary>
        /// Key used to save and load music volume from PlayerPrefs.
        /// </summary>
        protected const string MusicVolumePrefsKey = "Music Volume";
        /// <summary>
        /// Music volume in application. Max volume is value set in MaxVolume.
        /// </summary>
        protected static int _musicVolume;
    }
    
    /// <inheritdoc />
    public abstract class BaseMusicManager<T> : BaseMusicManager where T : MusicTrack, new()
    {
        /// <summary>
        /// Singleton.
        /// </summary>
        protected static BaseMusicManager<T> _instance;
        /// <summary>
        /// List of audio to use.
        /// </summary>
        [SerializeField, Tooltip("List of audio to use.")]
        protected List<T> _music = new List<T>();
        /// <summary>
        /// Should the music order be rearranged at start-up?
        /// </summary>
        [SerializeField, Tooltip("Should the music order be rearranged at start-up?")]
        protected bool _shuffle;
        /// <summary>
        /// AudioSource used to play music.
        /// </summary>
        protected AudioSource _audio;
        /// <summary>
        /// Index in list of the currently playing music.
        /// </summary>
        protected int _currentTrack;
        /// <summary>
        /// Event fired when the playing music is changed.
        /// </summary>
        public static event Action TrackChange = delegate { };
        /// <summary>
        /// Music volume in application. Max volume is value set in MaxVolume.
        /// </summary>
        public static int MusicVolume { get => _musicVolume; set => UpdateMusicVolume(value); }
        /// <summary>
        /// Information for the music currently playing.
        /// </summary>
        public static T CurrentlyPlaying => _instance._music[_instance._currentTrack];

        protected override void Awake()
        {
            //Destroy any other instance of this Component if a singleton has already been set.
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this);
            base.Awake();
            //Get AudioSource on GameObject or add one if none exists.
            if (!TryGetComponent(out _audio))
            {
                _audio = gameObject.AddComponent<AudioSource>();
            }
            //Get last saved volume, default to max volume if volume has not been set before.
            UpdateMusicVolume(PlayerPrefs.GetInt(MusicVolumePrefsKey, MaxVolume));
            //Only attempt to play if any tracks have been provided.
            if (_music.Count > 0)
            {
                Shuffle();
                PlayTrack();
            }
        }

        protected virtual void Update()
        {
            //If music has stopped and not because focus on the application has been lost, move onto next track in list.
            if (_music.Count > 0 && !_audio.isPlaying && _music[_currentTrack].Clip.loadState != AudioDataLoadState.Loading)
            {
                PlayNextTrack();
            }
        }

        /// <summary>
        /// Play the track before the currently playing track in the list.
        /// </summary>
        protected virtual void PlayPreviousTrack()
        {
            _currentTrack--;
            //Play last track in list if the first is currently playing.
            if (_currentTrack < 0)
            {
                _currentTrack = _music.Count - 1;
            }
            PlayTrack();
        }

        /// <summary>
        /// Play the track after the currently playing track in the list.
        /// </summary>
        protected virtual void PlayNextTrack()
        {
            _currentTrack++;
            //Play first track in list if the last is currently playing.
            if (_currentTrack >= _music.Count)
            {
                _currentTrack = 0;
            }
            PlayTrack();
        }

        /// <summary>
        /// Set-up and play the currently selected track.
        /// </summary>
        protected virtual void PlayTrack()
        {
            _audio.Stop();
            _audio.clip = _music[_currentTrack].Clip;
            _audio.volume = _music[_currentTrack].Volume * _musicVolume * (1f/MaxVolume);
            _audio.priority = _music[_currentTrack].Priority;
            _audio.Play();
            TrackChange?.Invoke();
        }

        /// <summary>
        /// Play the track which matches the name provided.
        /// </summary>
        /// <param name="trackName">Name of the track which should be played.</param>
        protected virtual void PlayTrack(string trackName)
        {
            var track = _music.IndexOf(_music.FirstOrDefault(t => t.TrackName == trackName));
            if (track < 0)
            {
                PlayTrack(track);
            }
            else
            {
                Debug.LogWarning("Invalid Track Name: " + trackName);
            }
        }

        /// <summary>
        /// Play the track which matches the AudioClip provided.
        /// AudioClip will be added if it not currently available as an option.
        /// </summary>
        /// <param name="clip">AudioClip of the track which should be played.</param>
        protected virtual void PlayTrack(AudioClip clip)
        {
            var track = _music.IndexOf(_music.FirstOrDefault(t => t.Clip == clip));
            if (track < 0)
            {
                PlayTrack(track);
            }
            else
            {
                if (clip != null)
                {
                    var newTrack = new T {Clip = clip};
                    _music.Add(newTrack);
                    Shuffle();
                    PlayTrack(_music.IndexOf(newTrack));
                    Debug.LogWarning("Invalid Track Clip: " + clip.name + ". New MusicTrack created for AudioClip as a result.");
                }
                else
                {
                    Debug.LogWarning("Invalid Track Clip provided");
                }
            }
        }

        /// <summary>
        /// Play the track in this index in the music list.
        /// </summary>
        /// <param name="trackNumber">Index of the track within the music list which should be played.</param>
        protected virtual void PlayTrack(int trackNumber)
        {
            if (trackNumber >= _music.Count || trackNumber < 0)
            {
                Debug.LogWarning("Invalid Track Number: " + trackNumber);
            }
            else
            {
               _currentTrack = trackNumber; 
               PlayTrack();
            }
        }

        /// <summary>
        /// Rearrange the order music will be played in.
        /// </summary>
        protected virtual void Shuffle()
        {
            if (_shuffle)
            {
                var position = _music.Count;
                while (position > 1)
                {
                    position--;
                    var random = UnityEngine.Random.Range(0, position + 1);
                    var stored = _music[random];
                    _music[random] = _music[position];
                    _music[position] = stored;
                }
                _currentTrack = UnityEngine.Random.Range(0, _music.Count);
            }
        }

        /// <summary>
        /// Play the track before the currently playing track in the list.
        /// </summary>
        public static void PreviousTrack()
        {
            _instance.PlayPreviousTrack();
        }

        /// <summary>
        /// Play the track after the currently playing track in the list.
        /// </summary>
        public static void NextTrack()
        {
            _instance.PlayNextTrack();
        }

        /// <summary>
        /// Play the track which matches the name provided.
        /// </summary>
        /// <param name="trackName">Name of the track which should be played.</param>
        public static void Play(string trackName)
        {
            _instance.PlayTrack(trackName);
        }

        /// <summary>
        /// Play the track which matches the AudioClip provided.
        /// AudioClip will be added if it not currently available as an option.
        /// </summary>
        /// <param name="clip">AudioClip of the track which should be played.</param>
        public static void Play(AudioClip clip)
        {
            _instance.PlayTrack(clip);
        }

        /// <summary>
        /// Play the track in this index in the music list.
        /// </summary>
        /// <param name="trackNumber">Index of the track within the music list which should be played.</param>
        public static void Play(int trackNumber)
        {
            _instance.PlayTrack(trackNumber);
        }

        /// <summary>
        /// Update the music volume of the application.
        /// </summary>
        /// <param name="amount">Set the volume value to this amount.</param>
        public static void UpdateMusicVolume(int amount)
        {
            var newVolume = Mathf.Clamp(amount, 0, MaxVolume);
            _musicVolume = newVolume;
            PlayerPrefs.SetInt(MusicVolumePrefsKey, newVolume);
            _instance._audio.volume = _instance._music[_instance._currentTrack].Volume * _musicVolume * (1f/MaxVolume);
        }
    }
}