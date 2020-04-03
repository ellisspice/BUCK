using BUCK.MenuSystem.MenuController;
using BUCK.MenuSystem.MenuController.OldInputSystem;
using UnityEngine.SceneManagement;

namespace BUCK.Examples.UnityUI
{
    public class PauseMenu : MenuBehaviour
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Gameplay.PlayingGame = false;
        }
        
        public void GoToGameplay()
        {
            _menuManager.SetState(MenuState.Gameplay);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToSettings()
        {
            _menuManager.SetState(MenuState.Settings);
        }

        public void Menu()
        {
            SceneManager.LoadScene("Menu");
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