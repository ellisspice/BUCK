using BUCK.MenuSystem.MenuController;
using BUCK.MenuSystem.MenuController.OldInputSystem;

namespace BUCK.Examples.UnityUI
{
    public class ControlsMenu : MenuBehaviour
    {
        public void GoToMain()
        {
            _menuManager.SetState(MenuState.Main);
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