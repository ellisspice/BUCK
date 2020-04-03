#if TEXT_MESH_PRO_INCLUDED
using System.Linq;
using BUCK.LocalizationSystem;
using TMPro;
using UnityEngine;

namespace BUCK.Examples
{
    public class TMProLocalizationExample : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private TMP_Dropdown _languageDropdown;

        private void Awake()
        {
            _languageDropdown.ClearOptions();
            _languageDropdown.AddOptions(Localization.Languages.Select(l => l.NativeName).ToList());
            _languageDropdown.value = _languageDropdown.options.FindIndex(l => l.text == Localization.SelectedLanguage.NativeName);
        }

        private void OnEnable()
        {
            Localization.LanguageChange += UpdateText;
            UpdateText();
        }

        private void OnDisable()
        {
            Localization.LanguageChange += UpdateText;
        }

        public void ChangeLanguage()
        {
            Localization.UpdateLanguage(Localization.Languages.FirstOrDefault(l => l.NativeName == _languageDropdown.options[_languageDropdown.value].text));
        }

        private void UpdateText()
        {
            _text.text = Localization.Get(LocalizationKeys.Hello);
        }
    }
}
#endif