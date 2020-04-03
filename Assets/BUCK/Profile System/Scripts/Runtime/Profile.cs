using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace BUCK.ProfileSystem
{
    /// <summary>
    /// Class defining a single user within the ProfileSystem.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Unique id for the Profile. Primarily used for saving and loading data.
        /// </summary>
        public int ProfileNumber { get; }
        /// <summary>
        /// Name of the Profile.
        /// </summary>
        protected string _name;
        /// <summary>
        /// Name of the Profile.
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = Set(ProfileConsts.Name, value);
        }
        /// <summary>
        /// Has this Profile been deleted, making it inaccessible to users?
        /// </summary>
        protected bool _deleted;
        /// <summary>
        /// Has this Profile been deleted, making it inaccessible to users?
        /// </summary>
        public bool Deleted
        {
            get => _deleted;
            set => _deleted = Set(ProfileConsts.Deleted, value);
        }

        /// <summary>
        /// Dictionary of data keys and their values.
        /// </summary>
        protected readonly Dictionary<string, string> _storedValues = new Dictionary<string, string>();

        /// <summary>
        /// Constructor for rebuilding a Profile that is being loaded.
        /// </summary>
        /// <param name="profileNumber">ID of the Profile being loaded.</param>
        public Profile(int profileNumber)
        {
            ProfileNumber = profileNumber;
            _name = GetString(ProfileConsts.Name);
        }

        /// <summary>
        /// Constructor for building a new Profile.
        /// </summary>
        /// <param name="profileName">Name of the Profile being created.</param>
        public Profile(string profileName)
        {
            ProfileNumber = ProfileManager.Profiles.Count;
            _name = profileName;
        }

        /// <summary>
        /// Load string data previously saved for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being loaded.</param>
        /// <returns>Returns the previously saved data for this key. Returns an empty string if no data exists for this key.</returns>
        public string GetString(string key)
        {
            //If no data is currently stored for this key, attempt to load from PlayerPrefs.
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, PlayerPrefs.GetString(ProfileKeyFormat(key), string.Empty));
            }
            return _storedValues[key];
        }

        /// <summary>
        /// Load int data previously saved for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being loaded.</param>
        /// <returns>Returns the previously saved data for this key. Returns 0 if no data exists for this key.</returns>
        public int GetInt(string key)
        {
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, PlayerPrefs.GetInt(ProfileKeyFormat(key), 0).ToString());
            }
            return int.Parse(_storedValues[key]);
        }

        /// <summary>
        /// Load float data previously saved for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being loaded.</param>
        /// <returns>Returns the previously saved data for this key. Returns 0 if no data exists for this key.</returns>
        public float GetFloat(string key)
        {
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, PlayerPrefs.GetFloat(ProfileKeyFormat(key), 0f).ToString(CultureInfo.InvariantCulture));
            }
            return float.Parse(_storedValues[key]);
        }

        /// <summary>
        /// Load bool data previously saved for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being loaded.</param>
        /// <returns>Returns the previously saved data for this key. Returns false if no data exists for this key.</returns>
        public bool GetBool(string key)
        {
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, (PlayerPrefs.GetInt(ProfileKeyFormat(key), 0) == 1).ToString());
            }
            return bool.Parse(_storedValues[key]);
        }

        /// <summary>
        /// Save string data for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being saved.</param>
        /// <param name="value">Value being saved for the key.</param>
        /// <returns>Returns the value which was saved.</returns>
        public string Set(string key, string value)
        {
            //Ensure data is in storedValues and up to date after saving.
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, value);
            }
            else
            {
                _storedValues[key] = value;
            }
            //Update value in PlayerPrefs.
            PlayerPrefs.SetString(ProfileKeyFormat(key), value);
            return value;
        }

        /// <summary>
        /// Save int data for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being saved.</param>
        /// <param name="value">Value being saved for the key.</param>
        /// <returns>Returns the value which was saved.</returns>
        public int Set(string key, int value)
        {
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, value.ToString());
            }
            else
            {
                _storedValues[key] = value.ToString();
            }
            PlayerPrefs.SetInt(ProfileKeyFormat(key), value);
            return value;
        }

        /// <summary>
        /// Save float data for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being saved.</param>
        /// <param name="value">Value being saved for the key.</param>
        /// <returns>Returns the value which was saved.</returns>
        public float Set(string key, float value)
        {
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                _storedValues[key] = value.ToString(CultureInfo.InvariantCulture);
            }
            PlayerPrefs.SetFloat(ProfileKeyFormat(key), value);
            return value;
        }

        /// <summary>
        /// Save bool data for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being saved.</param>
        /// <param name="value">Value being saved for the key.</param>
        /// <returns>Returns the value which was saved.</returns>
        public bool Set(string key, bool value)
        {
            if (!_storedValues.ContainsKey(key))
            {
                _storedValues.Add(key, value.ToString());
            }
            else
            {
                _storedValues[key] = value.ToString();
            }
            PlayerPrefs.SetInt(ProfileKeyFormat(key), value ? 1 : 0);
            return value;
        }
        
        /// <summary>
        /// Add value provided to the saved value for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being saved.</param>
        /// <param name="value">Value being added for the key.</param>
        /// <returns>Returns the value which was saved.</returns>
        public int Update(string key, int value)
        {
            var newValue = GetInt(key) + value;
            newValue = Set(key, newValue);
            return newValue;
        }

        /// <summary>
        /// Add value provided to the saved value for the key provided.
        /// </summary>
        /// <param name="key">Key of the data being saved.</param>
        /// <param name="value">Value being added for the key.</param>
        /// <returns>Returns the value which was saved.</returns>
        public float Update(string key, float value)
        {
            var newValue = GetFloat(key) + value;
            newValue = Set(key, newValue);
            return newValue;
        }

        /// <summary>
        /// Format the ProfileNumber and key into a string used for saving and loading.
        /// </summary>
        /// <param name="key">Key of the data being saved or loaded.</param>
        /// <returns>Formatting PlayerPrefs key containing the ProfileNumber and key.</returns>
        protected string ProfileKeyFormat(string key)
        {
            return ProfileKeyFormat(ProfileNumber, key);
        }
        
        /// <summary>
        /// Format the profileNumber and key into a string used for saving and loading.
        /// </summary>
        /// <param name="profileNumber">Profile number for which the data is being loaded or saved.</param>
        /// <param name="key">Key of the data being saved or loaded.</param>
        /// <returns>Formatting PlayerPrefs key containing the profileNumber and key.</returns>
        public static string ProfileKeyFormat(int profileNumber, string key)
        {
            return "Profile " + profileNumber + " " + key;
        }
    }
}