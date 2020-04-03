#if TEXT_MESH_PRO_INCLUDED
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BUCK.LocalizationSystem.TMPro
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Dropdown Localization (Text Mesh Pro)"), RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownLocalization : BaseDropdownLocalization
    {
        /// <inheritdoc />
        public override int Value => _dropdown ? _dropdown.value : -1;
        /// <summary>
        /// The Dropdown associated with this component.
        /// </summary>
        protected TMP_Dropdown _dropdown;
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<TMP_Dropdown.OptionData> options)
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
        public virtual void ReplaceOptions(IEnumerable<TMP_Dropdown.OptionData> options)
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
                var translatedOptions = Options.Select(t => new TMP_Dropdown.OptionData(Localization.Get(t.Key), t.Image)).ToList();
                _dropdown.AddOptions(translatedOptions);
                if (_dropdown.value > _dropdown.options.Count)
                {
                    _dropdown.value = 0;
                }
            }
        }
    }
}
#endif