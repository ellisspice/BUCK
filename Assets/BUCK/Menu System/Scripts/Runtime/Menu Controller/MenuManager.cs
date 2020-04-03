using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BUCK.MenuSystem.MenuController
{
    /// <inheritdoc />
    /// <summary>
    /// Menu UI state manager.
    /// </summary>
    [AddComponentMenu("UI/Menu Manager"), DisallowMultipleComponent]
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// List of MenuStateObjects which define the available states. Should only contain one object per state!
        /// </summary>
        [SerializeField, Tooltip("List of MenuStateObjects which define the available states. Should only contain one object per state!")]
        protected List<MenuStateObject> _menuStateList = new List<MenuStateObject>();
        /// <summary>
        /// Dictionary of MenuStates and their respective MenuBehaviour.
        /// </summary>
        protected Dictionary<MenuState, BaseMenuBehaviour> _menuStates = new Dictionary<MenuState, BaseMenuBehaviour>();
        /// <summary>
        /// The currently active state.
        /// </summary>
        [SerializeField, Tooltip("The currently active state.")]
        protected MenuState _activeState;

        protected virtual void Awake()
        {
            _menuStates = _menuStateList.ToDictionary(s => s.MenuState, s => s.GameObject);
            foreach (var state in _menuStates)
            {
                state.Value.gameObject.SetActive(false);
                state.Value.SetMenuManager(this);
            }
            SetState(_activeState);
        }

        /// <summary>
        /// Disable the currently active MenuState and enable the provided MenuState.
        /// </summary>
        /// <param name="activeState">MenuState to make active.</param>
        public virtual void SetState(MenuState activeState)
        {
            if (_menuStates.ContainsKey(activeState))
            {
                _menuStates[_activeState].gameObject.SetActive(false);
                _activeState = activeState;
                _menuStates[activeState].gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_menuStates[activeState].DefaultSelectable);
            }
            else
            {
                Debug.LogError(activeState + " is not a valid state for this MenuManager");
            }
        }

        /// <summary>
        /// Set the action to take when using the Cancel button in the currently active MenuState.
        /// </summary>
        /// <param name="action">Action to perform when going back from this state.</param>
        public virtual void SetBackAction(UnityAction action)
        {
            _menuStates[_activeState].SetBackAction(action);
        }
    }
}