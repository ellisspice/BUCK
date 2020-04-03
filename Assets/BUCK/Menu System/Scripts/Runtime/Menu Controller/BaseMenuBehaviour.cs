#if TEXT_MESH_PRO_INCLUDED
using TMPro;
#endif
#if BEST_FIT_TEXT_INCLUDED
using BUCK.BestFitText;
#endif
#if LOCALIZATION_SYSTEM_INCLUDED
using BUCK.LocalizationSystem;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BUCK.MenuSystem.MenuController
{
    /// <inheritdoc />
    /// <summary>
    /// Generic class for setting up a class related to a MenuState.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BaseMenuBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Selectable to focus on enabling this GameObject or if the EventSystem is currently selecting no object.
        /// </summary>
        [SerializeField, Tooltip("Selectable to focus on enabling this GameObject or if the EventSystem is currently selecting no object.")]
        protected GameObject _defaultSelectable;
        /// <summary>
        /// Action to perform when the cancel button or button associated with returning to the previous MenuState is pressed.
        /// </summary>
        [SerializeField, Tooltip("Action to perform when the cancel button or button associated with returning to the previous MenuState is pressed.")]
        protected UnityEvent _backAction;
        /// <summary>
        /// MenuManager for this MenuBehaviour.
        /// </summary>
        protected MenuManager _menuManager;
        /// <summary>
        /// Selectable to focus on enabling this GameObject or if the EventSystem is currently selecting no object.
        /// </summary>
        public GameObject DefaultSelectable => _defaultSelectable;

        protected virtual void Update()
        {
            if (BackButtonPressed())
            {
                //Partial bug created by this logic. This ensures that cancelling out of an InputField doesn't also trigger the Back method, but also means that the Back method cannot be triggered while the InputField remains focused.
                if (!EventSystem.current.currentSelectedGameObject || InputNotFocused())
                {
                    Back();
                }
            }
        }

        /// <summary>
        /// Is an InputField not the UI focus or was not cancelled out?
        /// </summary>
        /// <returns>Was an InputField not the UI focus or was not cancelled out?</returns>
        protected virtual bool InputNotFocused()
        {
            var notFocused = !EventSystem.current.currentSelectedGameObject.TryGetComponent<InputField>(out var currentInput) || !currentInput.wasCanceled;
#if TEXT_MESH_PRO_INCLUDED
            if (notFocused)
            {
                notFocused = !EventSystem.current.currentSelectedGameObject.TryGetComponent<TMP_InputField>(out var currentTMPInput) || !currentTMPInput.wasCanceled;
            }
#endif
            return notFocused;
        }

        /// <summary>
        /// Was the Cancel Button pressed this frame?
        /// </summary>
        /// <returns>Was the Cancel Button pressed this frame?</returns>
        protected abstract bool BackButtonPressed();

        /// <summary>
        /// Set the MenuManager for this MenuBehaviour.
        /// </summary>
        /// <param name="manager">MenuManager to assign to this MenuBehaviour.</param>
        public void SetMenuManager(MenuManager manager)
        {
            _menuManager = manager;
        }

        /// <summary>
        /// Set the action to take when using the Cancel button in this behaviour.
        /// </summary>
        /// <param name="action">Action to perform when going back from this state.</param>
        public virtual void SetBackAction(UnityAction action)
        {
            _backAction.RemoveAllListeners();
            _backAction.AddListener(action);
        }

        /// <summary>
        /// Invoke the set action for returning to the previous menu state.
        /// </summary>
        public virtual void Back()
        {
            _backAction?.Invoke();
        }
    }

    /// <inheritdoc />
    public abstract class BaseMenuBehaviour<T> : BaseMenuBehaviour where T : BaseInputModule
    {
        /// <summary>
        /// BaseInputModule within the current scene.
        /// </summary>
        protected T _input;
        
        protected virtual void OnEnable()
        {
            if (!_input)
            {
                _input = FindObjectOfType<T>();
            }
#if BEST_FIT_TEXT_INCLUDED
            BestFit.ResolutionChange += OnResolutionChange;
#endif
#if LOCALIZATION_SYSTEM_INCLUDED
            Localization.LanguageChange += OnLanguageChange;
#endif
        }
        
        protected virtual void OnDisable()
        {
#if BEST_FIT_TEXT_INCLUDED
            BestFit.ResolutionChange -= OnResolutionChange;
#endif
#if LOCALIZATION_SYSTEM_INCLUDED
            Localization.LanguageChange -= OnLanguageChange;
#endif
        }
        
#if BEST_FIT_TEXT_INCLUDED
        /// <summary>
        /// Method called when ResolutionChange (Best Fit Text asset) event is fired.
        /// </summary>
        protected abstract void OnResolutionChange();
#endif
        
#if LOCALIZATION_SYSTEM_INCLUDED
        /// <summary>
        /// Method called when LanguageChange (Localization System asset) event is fired.
        /// </summary>
        protected abstract void OnLanguageChange();
#endif
    }
}