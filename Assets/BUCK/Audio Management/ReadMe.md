# Audio Management
## Description
A set of managers and controllers which are used to handle the playing soundtrack and sound effect audio and to control audio volume.

## Example Usage
A MusicManager and SFXManager prefab has been provided in the "Examples" folder, both of which match the objects created from the "GameObject/Audio" menu. 

**Adjust overall application volume**

```csharp
void BaseAudioManager.UpdateVolume(int changeAmount);
```

All manager and controller classes inherit from BaseAudioManager, meaning all of the below are valid within those provided:

```csharp
void MusicManager.UpdateVolume(int changeAmount);
void SFXController.UpdateVolume(int changeAmount);
void SFXManager.UpdateVolume(int changeAmount);
```

### BaseMusicManager/MusicManager

BaseMusicManager is an abstract class with an generic MusicTrack type parameter which provides all of the basic functionality required for controlling a soundtrack, which the MusicManager class then directly inherits from, also using the MusicTrack type.

```csharp
// Play the previous track.
void MusicManager.PreviousTrack();

// Play the next track.
void MusicManager.NextTrack();

// Play the track with this name.
void MusicManager.Play(string trackName);

// Play the track with this clip.
void MusicManager.Play(AudioClip clip);

// Play the track of this index in the list of tracks.
void MusicManager.Play(int trackNumber);

// Update the music volume.
void MusicManager.UpdateMusicVolume(int changeAmount);
```

### BaseSFXController/SFXController

BaseSFXController is an abstract class with an generic SFXClip type parameter which provides all of the basic functionality required for controlling sound effects, which the SFXController class then inherits from, also using the SFXClip type.

```csharp
// Play the sound effect with this name.
void Play(string effectName, float pitch = 1f, float startVolume = -1f, float endVolume = -1f);

// Stop playing the sound effect with this name.
void Stop(string effectName);

// Stop playing all sound effects.
void StopAll();

// Pause the sound effect with this name.
void Pause(string effectName);

// Pause all sound effects.
void PauseAll();

// Resume playing the sound effect with this name.
void Resume(string effectName);

// Resume playing all sound effects.
void ResumeAll();

// Get if the sound effect with this name is currently being played.
bool IsPlaying(string effectName);

// Static method for updating sound effect volume.
void BaseSFXController.UpdateSFXVolume(int changeAmount);
```

### SFXManager

Like SFXController, SFXManager inherits from BaseSFXController. While SFXController is intended to be used to control sound effects related to a single GameObject, SFXManager is a singleton and as such is designed to be used to trigger sound effects globally within the application.

```csharp
// Static method for playing the sound effect with this name.
void SFXManager.PlaySound(string effectName, float pitch = 1f);

// Static method for playing the sound effect with this name at the given volumes.
void SFXManager.PlaySoundAtVolume(string effectName, float startVolume, float endVolume, float pitch = 1f);

// Static method to stop playing the sound effect with this name.
void SFXManager.StopSound(string effectName);

// Static method to stop playing all sound effects.
void SFXManager.StopAllSounds();

// Static method to pause the sound effect with this name.
void SFXManager.PauseSound(string effectName);

// Static method to pause all sound effects.
void SFXManager.PauseAllSounds();

// Static method to resume playing the sound effect with this name.
void SFXManager.ResumeSound(string effectName);

// Static method to resume playing all sound effects.
void SFXManager.ResumeAllSounds();

// Static method to get if the sound effect with this name is currently being played.
bool SFXManager.IsPlayingSound(string effectName);
```