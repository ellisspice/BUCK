#if NEW_INPUT_SYSTEM_INCLUDED
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace BUCK.MenuSystem.MenuController.NewInputSystem
{
    /// <inheritdoc />
    [AddComponentMenu("UI/Menu Behaviour (New Input System)")]
    public abstract class MenuBehaviour : BaseMenuBehaviour<InputSystemUIInputModule>
    {
        /// <inheritdoc />
        protected override bool BackButtonPressed()
        {
            return _input && _input.cancel.action.triggered;
        }
    }
}
#endif