using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BUCK.ProfileSystem
{
    /// <summary>
    /// Manager for loading and creating multiple profiles and determining which profiles should be used for Single Player and Multiplayer.
    /// </summary>
    public static class ProfileManager
    {
        /// <summary>
        /// Number of Multiplayer Profile slots for this application.
        /// </summary>
        public const int MultiplayerCap = 8;
        /// <summary>
        /// The current Profile being used for Single Player.
        /// </summary>
        public static Profile SinglePlayerProfile { get; private set; }
        /// <summary>
        /// The current Profiles being used for Multiplayer.
        /// </summary>
        public static Profile[] MultiPlayerProfiles { get; private set; } = new Profile[MultiplayerCap];
        /// <summary>
        /// All Profiles for this application, including those marked as deleted.
        /// </summary>
        public static List<Profile> Profiles { get; } = new List<Profile>();

        /// <summary>
        /// Constructor for ProfileSystem. Set-up profile system at start.
        /// </summary>
        static ProfileManager()
        {
            LoadProfileData();
        }

        /// <summary>
        /// Load existing (or create if it doesn't already exist) Profile data.
        /// </summary>
        public static void LoadProfileData()
        {
            //Ensure data is not duplicated.
            SinglePlayerProfile = null;
            MultiPlayerProfiles = new Profile[MultiplayerCap];
            Profiles.Clear();
            var profileNumber = 0;
            //Keep loading Profile data as long as a Name can be loaded for that ID.
            while (PlayerPrefs.HasKey(Profile.ProfileKeyFormat(profileNumber, ProfileConsts.Name)))
            {
                var loadedProfile = new Profile(profileNumber);
                Profiles.Add(loadedProfile);
                profileNumber++;
            }
            Debug.Log(Profiles.Count);
            //If no profiles exist, create a generic profile for up to the multiplayer cap.
            if (Profiles.Count == 0)
            {
                for (var i = 0; i < MultiplayerCap; i++)
                {
                    var newProfile = CreateProfile("Player " + (i + 1));
                    SetMultiPlayerProfile(i, newProfile.ProfileNumber);
                }
            }
            //Set the profile used for single player. Will default to the first non-deleted profile if not currently set.
            var singleSelected = PlayerPrefs.GetInt(ProfileConsts.SingleSelected, 0);
            SetSinglePlayerProfile(singleSelected);
            //Set the profiles used for multiplayer. Will default to -1 (aka, no profile set) if not currently set.
            for (var i = 0; i < MultiplayerCap; i++)
            {
                var multiSelected = PlayerPrefs.GetInt(ProfileConsts.MultiSelected + i, -1);
                SetMultiPlayerProfile(i, multiSelected);
            }
            Debug.Log(SinglePlayerProfile?.Name);
        }

        /// <summary>
        /// Set the Profile to use for Single Player.
        /// </summary>
        /// <param name="profileNumber">ID of the Profile.</param>
        public static void SetSinglePlayerProfile(int profileNumber)
        {
            //Ensure ID is valid and is not deleted, set to first not deleted otherwise.
            var matching = Profiles.FirstOrDefault(p => p.ProfileNumber == profileNumber);
            if (matching?.Deleted == false)
            {
                SinglePlayerProfile = matching;
            }
            else
            {
                //Create profile if all profiles are marked as deleted.
                if (Profiles.All(p => p.Deleted))
                {
                    CreateProfile("Player");
                }
                SinglePlayerProfile = Profiles.First(p => p.Deleted == false);
            }
            PlayerPrefs.SetInt(ProfileConsts.SingleSelected, SinglePlayerProfile.ProfileNumber);
        }

        /// <summary>
        /// Set the Profile to use in this Index within the Multiplayer Profiles.
        /// </summary>
        /// <param name="multiNumber">Index within the list of Multiplayer Profiles.</param>
        /// <param name="profileNumber">ID of the Profile.</param>
        public static void SetMultiPlayerProfile(int multiNumber, int profileNumber)
        {
            //Ensure multiNumber is valid, ignore request if not.
            if (multiNumber < 0 || multiNumber > MultiplayerCap - 1)
            {
                Debug.LogWarning(multiNumber + " is not a valid index for multiplayer");
                return;
            }
            //Ensure ID is valid and is not deleted, set to null otherwise.
            var matching = Profiles.FirstOrDefault(p => p.ProfileNumber == profileNumber);
            if (matching?.Deleted == false)
            {
                MultiPlayerProfiles[multiNumber] = matching;
            }
            else
            {
                MultiPlayerProfiles[multiNumber] = null;
            }
            PlayerPrefs.SetInt(ProfileConsts.MultiSelected + multiNumber, MultiPlayerProfiles[multiNumber]?.ProfileNumber ?? -1);
        }

        /// <summary>
        /// Mark the Profile as being deleted, making it unusable.
        /// </summary>
        /// <param name="profileNumber">ID of the Profile.</param>
        public static void DeleteProfile(int profileNumber)
        {
            var profile = Profiles.FirstOrDefault(p => p.ProfileNumber == profileNumber);
            if (profile != null)
            {
                profile.Deleted = true;
                //Reset Profiles used for Single Player and Multiplayer to ensure newly deleted profile is not used.
                SetSinglePlayerProfile(SinglePlayerProfile.ProfileNumber);
                for (var i = 0; i < MultiplayerCap; i++)
                {
                    SetMultiPlayerProfile(i, MultiPlayerProfiles[i]?.ProfileNumber ?? -1);
                }
            }
        }

        /// <summary>
        /// Create a new Profile with the name provided.
        /// </summary>
        /// <param name="profileName">Name of the new Profile.</param>
        /// <returns>Returns the newly created Profile.</returns>
        public static Profile CreateProfile(string profileName)
        {
            var newProfile = new Profile(profileName);
            Profiles.Add(newProfile);
            return newProfile;
        }
    }
}