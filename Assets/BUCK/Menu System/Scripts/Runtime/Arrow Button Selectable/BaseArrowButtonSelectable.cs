using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BUCK.MenuSystem.ArrowButtons
{
    /// <inheritdoc />
    /// <summary>
    /// Selectable made up of two buttons, designed to be used to cycle through values using left and right input or mouse/touch input.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BaseArrowButtonSelectable : Selectable
    {
        /// <summary>
        /// Empty option list.
        /// </summary>
        protected static readonly Dropdown.OptionData NoOptionData = new Dropdown.OptionData();
    }
    
    /// <inheritdoc />
    public abstract class BaseArrowButtonSelectable<T> : BaseArrowButtonSelectable where T : MaskableGraphic
    {
        /// <summary>
        /// Selectable left button. Events should not be directly set on buttons!
        /// </summary>
        [SerializeField, Tooltip("Selectable left button. Events should not be directly set on buttons!")]
        protected Button _leftButton;
        /// <summary>
        /// Selectable left button. Events should not be directly set on buttons!
        /// </summary>
        public Button LeftButton { get => _leftButton; set { _leftButton = value; AssignButtonListeners(); } }
        /// <summary>
        /// Selectable right button. Events should not be directly set on buttons!
        /// </summary>
        [SerializeField, Tooltip("Selectable right button. Events should not be directly set on buttons!")]
        protected Button _rightButton;
        /// <summary>
        /// Selectable right button. Events should not be directly set on buttons!
        /// </summary>
        public Button RightButton { get => _rightButton; set { _rightButton = value; AssignButtonListeners(); } }
        /// <summary>
        /// Text displayed for the currently selected value.
        /// </summary>
        [SerializeField, Tooltip("Text displayed for the currently selected value.")]
        protected T _selectedText;
        /// <summary>
        /// Text displayed for the currently selected value.
        /// </summary>
        public T SelectedText { get => _selectedText; set { _selectedText = value; RefreshShownValue(); } }
        /// <summary>
        /// Image displayed for the currently selected value.
        /// </summary>
        [SerializeField, Tooltip("Image displayed for the currently selected value.")]
        protected Image _selectedImage;
        /// <summary>
        /// Image displayed for the currently selected value.
        /// </summary>
        public Image SelectedImage { get => _selectedImage; set { _selectedImage = value; RefreshShownValue(); } }
        /// <summary>
        /// List of options for this selectable.
        /// </summary>
        [SerializeField, Tooltip("List of options for this selectable.")]
        protected Dropdown.OptionDataList _options = new Dropdown.OptionDataList();
        /// <summary>
        /// List of options for this selectable.
        /// </summary>
        public List<Dropdown.OptionData> Options { get => _options.options; set { _options.options = value; RefreshShownValue(); } }
        /// <summary>
        /// Event delegates triggered on arrow input.
        /// </summary>
        [SerializeField, Tooltip("Event delegates triggered on arrow input.")]
        protected ArrowEvent _onValueChanged = new ArrowEvent();
        /// <summary>
        /// Event delegates triggered on arrow input.
        /// </summary>
        public ArrowEvent OnValueChanged { get => _onValueChanged; set => _onValueChanged = value; }
        /// <summary>
        /// Currently selected value.
        /// </summary>
        protected int _selectedValue;
        /// <summary>
        /// Currently selected value.
        /// </summary>
        public int SelectedValue { get => _selectedValue; set => SetValue(value); }
        /// <summary>
        /// Should value loop back round to the other end of the options if SelectedValue go beyond available options?
        /// </summary>
        [SerializeField, Tooltip("Event delegates triggered on arrow input.")]
        protected bool _loopThrough;
        /// <summary>
        /// WaitForSeconds used to create delay when triggering fake button presses.
        /// </summary>
        protected readonly WaitForSeconds _wait = new WaitForSeconds(0.01f);
        /// <summary>
        /// The currently running button press.
        /// </summary>
        protected Coroutine _current;

        protected override void Awake()
        {
            base.Awake();
            if (!_leftButton)
            {
                FindSelectableOnLeft()?.TryGetComponent(out _leftButton);
            }
            if (!_rightButton)
            {
                FindSelectableOnRight()?.TryGetComponent(out _rightButton);
            }
            if (!_leftButton || !_rightButton)
            {
                interactable = false;
            }
            AssignButtonListeners();
        }

        protected virtual void AssignButtonListeners()
        {
            if (_leftButton)
            {
                _leftButton.onClick.RemoveAllListeners();
                _leftButton.onClick.AddListener(() => ChangeValue(-1));
            }
            if (_rightButton)
            {
                _rightButton.onClick.RemoveAllListeners();
                _rightButton.onClick.AddListener(() => ChangeValue(1));
            }
        }
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<Dropdown.OptionData> options)
        {
            Options.AddRange(options);
            RefreshShownValue();
        }
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<string> options)
        {
            foreach (var option in options)
            {
                Options.Add(new Dropdown.OptionData(option));
            }
            RefreshShownValue();
        }
        
        /// <summary>
        /// Add options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to add.</param>
        public virtual void AddOptions(IEnumerable<Sprite> options)
        {
            foreach (var option in options)
            {
                Options.Add(new Dropdown.OptionData(option));
            }
            RefreshShownValue();
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<Dropdown.OptionData> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<string> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <summary>
        /// Clear the current options and add provided options to the Options list.
        /// </summary>
        /// <param name="options">Collection of options to replace current list with.</param>
        public virtual void ReplaceOptions(IEnumerable<Sprite> options)
        {
            ClearOptions();
            AddOptions(options);
        }
        
        /// <summary>
        /// Clear the current options.
        /// </summary>
        public virtual void ClearOptions()
        {
            Options.Clear();
            _selectedValue = 0;
            RefreshShownValue();
        }
        
        /// <summary>
        /// Update the currently displayed text/image for the current selected value.
        /// </summary>
        public virtual Dropdown.OptionData RefreshShownValue()
        {
            var data = NoOptionData;

            if (Options.Count > 0)
            {
                data = Options[Mathf.Clamp(_selectedValue, 0, Options.Count - 1)];
            }

            if (_selectedImage)
            {
                _selectedImage.sprite = data?.image;
                _selectedImage.enabled = _selectedImage.sprite;
            }

            return data;
        }
        
        /// <summary>
        /// Trigger button presses if input is left/right, navigate to different selectable as normal if input is up/down
        /// </summary>
        public override void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Right:
                    if (_current == null)
                    {
                        _current = StartCoroutine(FakeClickButton(_rightButton));
                    }
                    break;
                case MoveDirection.Up:
                    Navigate(eventData, FindSelectableOnUp());
                    break;
                case MoveDirection.Left:
                    if (_current == null)
                    {
                        _current = StartCoroutine(FakeClickButton(_leftButton));
                    }
                    break;
                case MoveDirection.Down:
                    Navigate(eventData, FindSelectableOnDown());
                    break;
                case MoveDirection.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventData.moveDir), eventData.moveDir, null);
            }
        }
        
        /// <summary>
        /// Set the currently selected value.
        /// </summary>
        /// <param name="value">Value to set SelectedValue to.</param>
        public virtual void SetValue(int value)
        {
            if (Application.isPlaying && (value == _selectedValue || Options.Count == 0))
            {
                return;
            }

            _selectedValue = _loopThrough ? (value < 0 ? value + Options.Count : value) % Options.Count : Mathf.Clamp(value, 0, Options.Count - 1);
            RefreshShownValue();

            //Notify all listeners.
            UISystemProfilerApi.AddMarker("Arrow.value", this);
            _onValueChanged.Invoke(_selectedValue);
        }

        /// <summary>
        /// Change the currently selected value by the amount provided.
        /// </summary>
        /// <param name="value">Value to adjust SelectedValue by.</param>
        public virtual void ChangeValue(int value)
        {
            SetValue(_selectedValue + value);
        }

        /// <summary>
        /// Execute various events on the selected button in order to fake a button press, thus triggering the button's event and animation
        /// </summary>
        protected virtual IEnumerator FakeClickButton(Button button)
        {
            if (button)
            {
                //Simulate a button click.  
                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerDownHandler);
                yield return _wait;
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerExitHandler);
                button.onClick.Invoke();
            }
            _current = null;
        }

        /// <summary>
        /// Navigate to Selectable if one exists
        /// </summary>
        protected virtual void Navigate(AxisEventData eventData, Selectable sel)
        {
            if (sel)
            {
                eventData.selectedObject = sel.gameObject;
            }
        }
    }
}