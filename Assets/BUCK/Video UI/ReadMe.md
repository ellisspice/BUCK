# Video UI
## Description
A set of components related to playing videos within Unity's UI system.

The BaseVideoPlayer provides all of the basic functionality required to have a video played on a RawImage. All provided components inherit from this class.

The AutoVideoPlayer is used to display an auto-playing, looping video whenever input inactivity for a set period of time is detected. This component is primarily designed to be used to provide an "attract screen" or demo mode.

The VideoPlayerUI is designed to provide most of the functionality found in a typical video player online, such as pausing, muting and setting video volume, playback position and playback speed.
 
The VideoPlaylistPlayerUI inherits from VideoPlayerUI and provides the same functionality for playing a set of videos, with functionality for go back or forward in the list of videos provided.

## Example Usage
An example prefab for each of the provided components, as well as scenes containing all of these prefabs, can be found in the "Examples" folder. Please note that sprites have not been provided for the buttons contained within the "Video Player" and "Video Playlist Player" prefabs. GameObjects similar to these prefabs (with the required components and without any UI elements) can also be created via the "GameObject/Video" menu as well.

### Base Video Player
```csharp
// Set the video to be played.
void SetVideo(Video video);

// Play the current video with the settings set on this component.
void PlayWithPredefinedSettings();

// Play the current video with the current settings of the VideoPlayer and AudioSource components.
void PlayWithCurrentSettings();

// Play the current video with the provided settings.
void Play(bool loop = false, float playbackSpeed = 1f, float volume = 1f, bool mute = false, bool skipOnDrop = true, bool playOnAwake = true, bool waitForFirstFrame = true);

// Continue (or start if not) playing the currently playing video.
void Continue();

// Pause the currently playing video.
void Pause();

// Stop the currently playing video.
void Stop();

// Update the VideoPlayer and AudioSource settings to those provided.
void SetSettings(bool loop, float playbackSpeed, float volume, bool mute, bool skipOnDrop, bool playOnAwake, bool waitForFirstFrame);

// Mute the AudioSource.
void Mute();

// Unmute the AudioSource.
void Unmute();
```
### Video Player UI
```csharp
// Play the current video if currently paused and vice versa.
void PlayToggle();

// Update the current time into the video to match the value of the position slider.
void UpdateVideoPosition();

// Update the current video volume to match the value of the slider.
void UpdateVolume();

// Update the current playback speed to match the current dropdown value.
void UpdatePlaybackSpeed();

// Loop the video.
void Loop();

// Do not loop the video.
void Unoop();
```

### Video Playlist Player UI
```csharp
// Play the previous video in the playlist if early in the video, or restart the current video if not.
void GoToPrevious();

// Play the next video in the playlist.
void GoToNext();
```

## Potential Issues

- Video playback and UI has been observed to lag if playback speed is set to be greater than 5.
- Error message "Can't play movie []" appears whenever the "Stop" method is called or whenever a VideoClip is played as the video URL is set to be empty. This error does not affect application running at all.