#if LOCALIZATION_SYSTEM_INCLUDED
using BUCK.LocalizationSystem;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.MenuSystem.ArrowButtons.UnityUI
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Arrow Button Selectable")]
    public class ArrowButtonSelectable : BaseArrowButtonSelectable<Text>
    {
#if LOCALIZATION_SYSTEM_INCLUDED
        /// <summary>
        /// Should option text values be used for localization? Displayed text will instead be the localized text related to the current value.
        /// </summary>
        [SerializeField, Tooltip("Should option text values be used for localization? Displayed text will instead be the localized text related to the current value.")]
        protected bool _localized = true;
#endif
        
        /// <inheritdoc />
        public override Dropdown.OptionData RefreshShownValue()
        {
            var data = base.RefreshShownValue();
            if (_selectedText)
            {
#if LOCALIZATION_SYSTEM_INCLUDED
                _selectedText.text = _localized ? Localization.Get(data?.text ?? string.Empty) : data?.text ?? string.Empty;
#else
                _selectedText.text = data?.text ?? string.Empty;
#endif
            }
            if (_selectedImage)
            {
                _selectedImage.sprite = data?.image;
            }
            return data;
        }
    }
}