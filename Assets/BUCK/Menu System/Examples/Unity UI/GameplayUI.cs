using System.Globalization;
using BUCK.MenuSystem.MenuController;
using BUCK.MenuSystem.MenuController.OldInputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.Examples.UnityUI
{
    public class GameplayUI : MenuBehaviour
    {
        [SerializeField]
        private Gameplay _gameplay;
        [SerializeField]
        private Text _timerText;

        private void Start()
        {
            Gameplay.GameOngoing = true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Gameplay.PlayingGame = true;
        }

        protected override void Update()
        {
            base.Update();
            _timerText.text = Mathf.Ceil(_gameplay.GameTimer).ToString(CultureInfo.InvariantCulture);
            if (_gameplay.GameTimer < 0)
            {
                _menuManager.SetState(MenuState.GameEnd);
            }
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