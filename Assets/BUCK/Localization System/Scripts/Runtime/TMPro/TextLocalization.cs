#if TEXT_MESH_PRO_INCLUDED
using TMPro;
using UnityEngine;

namespace BUCK.LocalizationSystem.TMPro
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Text Localization (Text Mesh Pro)"), RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocalization : BaseTextLocalization
    {
        /// <summary>
        /// The Text associated with this component.
        /// </summary>
        protected TextMeshProUGUI _text;

        /// <inheritdoc />
        public override void Set()
        {
            if (!_text)
            {
                _text = GetComponent<TextMeshProUGUI>();
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
#endif