using UnityEngine;
using UnityEngine.EventSystems;

namespace BUCK.MenuSystem.MenuController.OldInputSystem
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Menu Behaviour")]
    public abstract class MenuBehaviour : BaseMenuBehaviour<StandaloneInputModule>
    {
        /// <inheritdoc />
        protected override bool BackButtonPressed()
        {
            return _input && Input.GetButtonDown(_input.cancelButton);
        }
    }
}