# Profile System
## Description
A system for saving and loading data for multiple users in a single application.

## Example Usage
### Profile

**Load Data**

```csharp
string GetString(string key);
int GetInt(string key);
float GetFloat(string key);
bool GetBool(string key);
```

**Save Data**

```csharp
string Set(string key, string value);
int Set(string key, int value);
float Set(string key, float value);
bool Set(string key, bool value);
int Update(string key, int value);
float Update(string key, float value);
```

### ProfileSystem

```csharp
// Static method for loading/reloading all profile data. Automatically called at the start of the application.
void ProfileSystem.LoadProfileData();

// Static method for setting the profile marked as for use in Single Player.
void ProfileSystem.SetSinglePlayerProfile(int profileNumber);

// Static method for setting a profile marked as for use in Multiplayer.
void ProfileSystem.SetMultiPlayerProfile(int multiNumber, int profileNumber);

// Static method for marking a profile as being deleted, making it no longer possible to use for Single Player or Multiplayer.
void ProfileSystem.DeleteProfile(int profileNumber);

// Static method to create a new profile.
void ProfileSystem.CreateProfile(string profileName);
```