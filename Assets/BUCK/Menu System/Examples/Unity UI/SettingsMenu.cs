using System.Linq;
using System.Collections;
using BUCK.MenuSystem.ArrowButtons.UnityUI;
using BUCK.MenuSystem.MenuController;
using BUCK.MenuSystem.MenuController.OldInputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BUCK.Examples.UnityUI
{
    public class SettingsMenu : MenuBehaviour
    {
        [SerializeField]
        private GameObject _standaloneSettings;
        [SerializeField]
        private GameObject _buttonsPreChange;
        [SerializeField]
        private GameObject _buttonsPostChange;
        [SerializeField]
        private ArrowButtonSelectable _resolution;
        [SerializeField]
        private Toggle _fullScreen;
        [SerializeField]
        private ArrowButtonSelectable _volume;
        [SerializeField]
        private GameObject _keepButton;
        [SerializeField]
        private Text _timerText;
        private int _previousSelectedResolution;
        private bool _previousFullScreen;
        private int _previousVolume;
        private float _revertTimer = -5;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                StartCoroutine(MacResolutionFix());
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ResetSettings();
        }

        protected override void Update()
        {
            base.Update();
            if (_revertTimer >= 0)
            {
                _revertTimer -= Time.smoothDeltaTime;
                _timerText.text = $"Keep the changes? Will automatically reset to previous settings in {Mathf.CeilToInt(_revertTimer)} seconds.";
            }
            else if (_revertTimer < 0 && _revertTimer > -1)
            {
                RevertSettings();
            }
        }

        public void ApplySettings()
        {
            _previousVolume = Mathf.RoundToInt(AudioListener.volume * 10);
            AudioListener.volume = _volume.SelectedValue * 0.1f;
            if (_standaloneSettings.activeSelf)
            {
                var currentRes = new Resolution {height = Screen.height, width = Screen.width};
                _previousSelectedResolution = _resolution.Options.FindIndex(o => o.text == currentRes.width + " x " + currentRes.height);
                _previousFullScreen = Screen.fullScreen;
                Screen.SetResolution(int.Parse(_resolution.Options[_resolution.SelectedValue].text.Split('x')[0]), int.Parse(_resolution.Options[_resolution.SelectedValue].text.Split('x')[1]), _fullScreen.isOn);
            }
            _revertTimer = 15;
            _buttonsPreChange.SetActive(false);
            _buttonsPostChange.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_keepButton);
        }

        public void ResetSettings()
        {
            _revertTimer = -5;
            _timerText.text = string.Empty;
            _buttonsPreChange.SetActive(true);
            _buttonsPostChange.SetActive(false);
            _standaloneSettings.SetActive(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor);
            if (_standaloneSettings.activeSelf)
            {
                var resolutions = Screen.resolutions.Distinct().ToList();
                var current = new Resolution {height = Screen.height, width = Screen.width};
                if (!resolutions.Any(res => res.width == current.width && res.height == current.height))
                {
                    resolutions.Add(current);
                }
                resolutions = resolutions.OrderBy(res => res.width).ThenBy(res => res.height).ToList();
                _resolution.ReplaceOptions(resolutions.Select(res => res.width + " x " + res.height).Distinct().ToList());
                _resolution.SetValue(_resolution.Options.FindIndex(o => o.text == current.width + " x " + current.height));
                _fullScreen.isOn = Screen.fullScreen;
                _previousFullScreen = Screen.fullScreen;
                _previousSelectedResolution = _resolution.SelectedValue;
            }
            _volume.SelectedValue = Mathf.RoundToInt(AudioListener.volume * 10);
            _previousVolume = _volume.SelectedValue;
        }

        public void KeepSettings()
        {
            ResetSettings();
            EventSystem.current.SetSelectedGameObject(_defaultSelectable);
        }

        public void RevertSettings()
        {
            if (_standaloneSettings.activeSelf)
            {
                _resolution.SelectedValue = _previousSelectedResolution;
                _fullScreen.isOn = _previousFullScreen;
                Screen.SetResolution(int.Parse(_resolution.Options[_resolution.SelectedValue].text.Split('x')[0]), int.Parse(_resolution.Options[_resolution.SelectedValue].text.Split('x')[1]), _fullScreen.isOn);
            }
            _volume.SelectedValue = _previousVolume;
            AudioListener.volume = _volume.SelectedValue * 0.1f;
            _revertTimer = -5;
            _buttonsPreChange.SetActive(true);
            _buttonsPostChange.SetActive(false);
            _timerText.text = string.Empty;
            EventSystem.current.SetSelectedGameObject(_defaultSelectable);
        }

        private static IEnumerator MacResolutionFix()
        {
            if (!Screen.fullScreen)
            {
                yield break;
            }
            var resolutions = Screen.resolutions;
            if (resolutions.Length > 0 && resolutions[resolutions.Length - 1].width > 2048)
            {
                Screen.fullScreen = false;
                yield return new WaitForEndOfFrame();
                Screen.fullScreen = true;
                yield return new WaitForEndOfFrame();
            }
        }
        
        public void GoToMain()
        {
            _menuManager.SetState(MenuState.Main);
        }
        
        public void GoToPause()
        {
            _menuManager.SetState(MenuState.Pause);
        }
        
#if BEST_FIT_TEXT_INCLUDED
        protected override void OnResolutionChange()
        {

        }
#endif
        
#if LOCALIZATION_SYSTEM_INCLUDED
        protected override void OnLanguageChange()
        {

        }
#endif
    }
}