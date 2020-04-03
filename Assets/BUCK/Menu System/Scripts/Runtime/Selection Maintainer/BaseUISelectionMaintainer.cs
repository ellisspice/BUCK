using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if TEXT_MESH_PRO_INCLUDED
using TMPro;
#endif

namespace BUCK.MenuSystem.SelectionMaintainer
{
    /// <inheritdoc />
    /// <summary>
    /// Determine the last input used and ensure a UI option is selected at all times when using controller/keyboard.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BaseUISelectionMaintainer : MonoBehaviour
    {
        /// <summary>
        /// Last recorded position of the mouse before moving over to ControllerInput being true.
        /// </summary>
        protected static Vector3 _lastMousePosition;
        /// <summary>
        /// Is a controller/keyboard currently being used for input?
        /// </summary>
        public static bool ControllerInput { get; protected set; }
    }
    
    /// <inheritdoc />
    public abstract class BaseUISelectionMaintainer<T> : BaseUISelectionMaintainer where T : BaseInputModule
    {
        /// <summary>
        /// Last selected GameObject Selectable.
        /// </summary>
        protected Selectable _lastSelected;
#if TEXT_MESH_PRO_INCLUDED
        /// <summary>
        /// Last selected GameObject InputField (if any).
        /// </summary>
        protected TMP_InputField _selectedTMPInput;
#endif
        /// <summary>
        /// Last selected GameObject InputField (if any).
        /// </summary>
        protected InputField _selectedInput;
        /// <summary>
        /// Has an input been made that should result in a different object being selected?
        /// </summary>
        protected bool _inputChange;
        /// <summary>
        /// Direction of the previous input.
        /// </summary>
        protected string _lastInput;
        /// <summary>
        /// Direction of the previous input made before the previous input where the axis used was different.
        /// </summary>
        protected string _lastDifferentInput;
        /// <summary>
        /// BaseInputModule within the current scene.
        /// </summary>
        protected T _input;
        /// <summary>
        /// Is the user allowed to use mouse input?
        /// </summary>
        [SerializeField, Tooltip("Is the user allowed to use mouse input?")]
        protected bool _allowMouseInput = true;
        /// <summary>
        /// Event triggered when the input type has changed.
        /// </summary>
        public static event Action InputChange = delegate { };

        protected virtual void Start()
        {
            _input = FindObjectOfType<T>();
            GetInputStartState();
            SetControllerInput(ControllerInput);
        }

        protected virtual void OnDisable()
        {
            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        protected virtual void Update()
        {
            GetCurrentInputType();
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            ChangeSelected(currentSelected);
            //If the recorded last selected does not match the currently selected, update values.
            if (currentSelected && (!_lastSelected || _lastSelected.gameObject != currentSelected))
            {
                currentSelected.TryGetComponent(out _lastSelected);
                _lastSelected.TryGetComponent(out _selectedInput);
#if TEXT_MESH_PRO_INCLUDED
                _lastSelected.TryGetComponent(out _selectedTMPInput);
#endif
            }
            //If no currently using controller input, stop as focusing something to be selected is no longer needed.
            if (!ControllerInput)
            {
                return;
            }
            //If nothing is currently selected, set last recorded selected object to be selected if still valid.
            if (!currentSelected)
            {
                EventSystem.current.SetSelectedGameObject(ValidSelectable(_lastSelected) ? _lastSelected.gameObject : null);
            }
            //If an object is currently selected, check for axis inputs.
            if (currentSelected)
            {
                var currentDirection = FindCurrentDirection();
                if (!string.IsNullOrEmpty(currentDirection))
                {
                    //If an InputField is currently selected and being typed in, ignore all directional input.
                    if (_selectedInput && _selectedInput.isFocused)
                    {
                        return;
                    }
#if TEXT_MESH_PRO_INCLUDED
                    if (_selectedTMPInput && _selectedTMPInput.isFocused)
                    {
                        return;
                    }
#endif
                    if (_lastInput != currentDirection && _lastInput != OppositeDirection(currentDirection))
                    {
                        _lastDifferentInput = _lastInput;
                    }
                    _lastInput = currentDirection;
                    _inputChange = true;
                }
            }
        }

        /// <summary>
        /// Set-up the controller setting at application start-up.
        /// </summary>
        protected abstract void GetInputStartState();

        /// <summary>
        /// Update the controller setting based on input received this frame and application settings.
        /// </summary>
        protected abstract void GetCurrentInputType();

        /// <summary>
        /// Update the selected object if the currently selected is not valid and an input has been received.
        /// </summary>
        /// <param name="currentSelected">The currently selected GameObject.</param>
        protected virtual void ChangeSelected(GameObject currentSelected)
        {
            if (!currentSelected)
            {
                return;
            }
            currentSelected.TryGetComponent(out Selectable currentSelectable);
            if (!currentSelectable)
            {
                return;
            }
            var startingSelectable = currentSelectable;
            while (_inputChange && !string.IsNullOrEmpty(_lastInput) && !ValidSelectable(currentSelectable))
            {
                var currentDirSelectable = FindSelectableForDirection(currentSelectable, _lastInput);
                var oppositeDirection = OppositeDirection(_lastInput);
                var oppositeDirSelectable = FindSelectableForDirection(currentSelectable, oppositeDirection);
                var startingOppositeDirSelectable = FindSelectableForDirection(startingSelectable, oppositeDirection);
                //If there is a selectable in the current direction, set it as the object to select.
                if (currentDirSelectable)
                {
                    currentSelectable = currentDirSelectable;
                }
                //Otherwise, if there is a selectable available in the opposite direction, set it as the object to select.
                else if (ValidSelectable(oppositeDirSelectable))
                {
                    _lastInput = oppositeDirection;
                    currentSelectable = oppositeDirSelectable;
                }
                //Otherwise, if there is a selectable available in the opposite direction to the starting selected object, set it as the object to select.
                else if (ValidSelectable(startingOppositeDirSelectable))
                {
                    _lastInput = oppositeDirection;
                    currentSelectable = startingOppositeDirSelectable;
                }
                //Otherwise, change input direction to last different direction (aka left and right change to up or down and vice versa) and set starting object as the object to select.
                else
                {
                    _lastInput = _lastDifferentInput;
                    _lastDifferentInput = string.Empty;
                    currentSelectable = startingSelectable;
                }
            }
            //If the final result is different to the object selected at the start, change selected GameObject.
            if (_inputChange && currentSelectable != startingSelectable)
            {
                EventSystem.current.SetSelectedGameObject(currentSelectable.gameObject);
            }
            _inputChange = false;
        }

        /// <summary>
        /// Set if the current input is controller/keyboard or mouse/touch.
        /// </summary>
        /// <param name="controller">Is the input controller/keyboard based?</param>
        protected virtual void SetControllerInput(bool controller)
        {
            ControllerInput = controller;
            Cursor.visible = !controller;
            SetLastMousePosition();
            if (!controller)
            {
                if (EventSystem.current.currentSelectedGameObject)
                {
                    EventSystem.current.currentSelectedGameObject.TryGetComponent(out _lastSelected);
                }
                EventSystem.current.SetSelectedGameObject(null);
            }
            InputChange?.Invoke();
        }

        /// <summary>
        /// Get and store the current position of the mouse or the last touch input.
        /// </summary>
        protected abstract void SetLastMousePosition();

        /// <summary>
        /// Get the Selectable for the provided Selectable in the provided input direction.
        /// </summary>
        /// <param name="selectable">Selectable to get navigation object for.</param>
        /// <param name="direction">Current input direction.</param>
        /// <returns>Returns the Selectable that should be selected for the current input.</returns>  
        protected virtual Selectable FindSelectableForDirection(Selectable selectable, string direction)
        {
            switch (direction)
            {
                case "left":
                    return selectable.FindSelectableOnLeft();
                case "right":
                    return selectable.FindSelectableOnRight();
                case "down":
                    return selectable.FindSelectableOnDown();
                case "up":
                    return selectable.FindSelectableOnUp();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get the current input direction.
        /// </summary>
        /// <returns>Returns a string matching the current input.</returns>  
        protected abstract string FindCurrentDirection();

        /// <summary>
        /// Get the opposite of the current input direction.
        /// </summary>
        /// <returns>Returns a string matching the opposite of the current input.</returns>  
        protected virtual string OppositeDirection(string direction)
        {
            switch (direction)
            {
                case "left":
                    return "right";
                case "right":
                    return "left";
                case "down":
                    return "up";
                case "up":
                    return "down";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Is the provided Selectable not null, active in the hierarchy and interactable?
        /// </summary>
        /// <param name="selectable">Selectable to check conditions for.</param>
        protected virtual bool ValidSelectable(Selectable selectable)
        {
            return selectable && selectable.gameObject.activeInHierarchy && selectable.interactable;
        }
    }
}