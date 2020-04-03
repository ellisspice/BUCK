using System;
using UnityEngine;

namespace BUCK.LocalizationSystem
{
    /// <summary>
    /// Class to store the text and/or image of a single option in the dropdown list.
    /// </summary>
    [Serializable]
    public class LocalizationOptionData
    {
        /// <summary>
        /// The Localization key associated with the option.
        /// </summary>
        [SerializeField, Tooltip("The Localization key associated with the option.")]
        protected string _key;
        /// <summary>
        /// The image associated with the option.
        /// </summary>
        [SerializeField, Tooltip("The image associated with the option.")]
        protected Sprite _image;

        /// <summary>
        /// The Localization key associated with the option.
        /// </summary>
        public string Key { get => _key; set => _key = value; }

        /// <summary>
        /// The image associated with the option.
        /// </summary>
        public Sprite Image { get => _image; set => _image = value; }

        public LocalizationOptionData()
        {
        }

        public LocalizationOptionData(string key)
        {
            Key = key;
        }

        public LocalizationOptionData(Sprite image)
        {
            Image = image;
        }

        /// <summary>
        /// Create an object representing a single option for the dropdown list.
        /// </summary>
        /// <param name="key">Optional key for the option.</param>
        /// <param name="image">Optional image for the option.</param>
        public LocalizationOptionData(string key, Sprite image)
        {
            Key = key;
            Image = image;
        }
    }
}