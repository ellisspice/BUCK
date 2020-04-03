using BUCK.MenuSystem.MenuController;
using BUCK.MenuSystem.MenuController.OldInputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.Examples.UnityUI
{
    public class MainMenu : MenuBehaviour
    {
        [SerializeField]
        private GameObject _exitButton;

        private void Start()
        {
            _exitButton.SetActive(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer);
        }
        
        public void GoToGameplay()
        {
            SceneManager.LoadScene("Gameplay");
        }
        
        public void GoToSettings()
        {
            _menuManager.SetState(MenuState.Settings);
        }
        
        public void GoToHelp()
        {
            _menuManager.SetState(MenuState.Help);
        }
        
        public void GoToControls()
        {
            _menuManager.SetState(MenuState.Controls);
        }
        
        public void GoToCredits()
        {
            _menuManager.SetState(MenuState.Credits);
        }
        
        public void Exit()
        {
            Application.Quit();
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