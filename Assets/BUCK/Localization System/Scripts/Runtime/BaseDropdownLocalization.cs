#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections.Generic;
using UnityEngine;

namespace BUCK.LocalizationSystem
{
    /// <inheritdoc />
    /// <summary>
    /// Class for setting the options on a Dropdown using the list of keys and sprites provided. 
    /// </summary>
    public abstract class BaseDropdownLocalization : UILocalization
    {
        /// <summary>
        /// The localization options for this dropdown.
        /// </summary>
        [SerializeField, Tooltip("The localization options for this dropdown.")]
        protected LocalizationOptionDataList _options = new LocalizationOptionDataList();
        /// <summary>
        /// The localization options for this dropdown.
        /// </summary>
        public List<LocalizationOptionData> Options { get => _options.Options; set { _options.Options = value; Set(); } }
        
        /// <summary>
        /// Currently selected value on the Dropdown.
        /// </summary>
        public virtual int Value { get; protected set; }

        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<LocalizationOptionData> options)
        {
            Options.AddRange(options);
            Set();
        }
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<string> options)
        {
            foreach (var option in options)
            {
                Options.Add(new LocalizationOptionData(option));
            }
            Set();
        }
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<Sprite> options)
        {
            foreach (var option in options)
            {
                Options.Add(new LocalizationOptionData(option));
            }
            Set();
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<LocalizationOptionData> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<string> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<Sprite> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <summary>
        /// Clear the current options.
        /// </summary>
        public virtual void ClearOptions()
        {
            Options.Clear();
            Set();
        }
    }
}