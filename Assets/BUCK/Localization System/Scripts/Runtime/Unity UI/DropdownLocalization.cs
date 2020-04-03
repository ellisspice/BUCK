#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.LocalizationSystem.UnityUI
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Dropdown Localization"), RequireComponent(typeof(Dropdown))]
    public class DropdownLocalization : BaseDropdownLocalization
    {
        /// <inheritdoc />
        public override int Value => _dropdown ? _dropdown.value : -1;
        /// <summary>
        /// The Dropdown associated with this component.
        /// </summary>
        protected Dropdown _dropdown;
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<Dropdown.OptionData> options)
        {
            foreach (var option in options)
            {
                Options.Add(new LocalizationOptionData(option.text, option.image));
            }
            Set();
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<Dropdown.OptionData> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Update the options on the associated Dropdown to the current language.
        /// </summary>
        public override void Set()
        {
            if (_options != null)
            {
                if (!_dropdown)
                {
                    TryGetComponent(out _dropdown);
                }
                if (!_dropdown)
                {
                    Debug.LogError("Localization script could not find Dropdown component attached to gameObject: " + gameObject.name);
                    return;
                }
                _dropdown.ClearOptions();
                var translatedOptions = Options.Select(t => new Dropdown.OptionData(Localization.Get(t.Key), t.Image)).ToList();
                _dropdown.AddOptions(translatedOptions);
                if (_dropdown.value > _dropdown.options.Count)
                {
                    _dropdown.value = 0;
                }
            }
        }
    }
}