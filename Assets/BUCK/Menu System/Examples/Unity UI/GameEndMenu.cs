using BUCK.MenuSystem.MenuController.OldInputSystem;
using UnityEngine.SceneManagement;

namespace BUCK.Examples.UnityUI
{
    public class GameEndMenu : MenuBehaviour
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Gameplay.GameOngoing = false;
            Gameplay.PlayingGame = false;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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