#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.LocalizationSystem.UnityUI
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Text Localization"), RequireComponent(typeof(Text))]
    public class TextLocalization : BaseTextLocalization
    {
        /// <summary>
        /// The Text associated with this component.
        /// </summary>
        protected Text _text;

        /// <inheritdoc />
        public override void Set()
        {
            if (!_text)
            {
                _text = GetComponent<Text>();
            }
            if (!_text)
            {
                Debug.LogError("Localization script could not find Text component attached to gameObject: " + gameObject.name);
                return;
            }
            _text.text = Localization.GetAndFormat(Key, Application.isEditor && !Application.isPlaying ? LanguageOverride : null, ToUpper, FormatArgs);
        }
    }
}