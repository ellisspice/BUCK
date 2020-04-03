using System.Linq;
using BUCK.LocalizationSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.Examples
{
    public class LocalizationExample : MonoBehaviour
    {
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Dropdown _languageDropdown;

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
