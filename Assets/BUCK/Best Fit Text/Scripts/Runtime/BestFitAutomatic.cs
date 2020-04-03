#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;
#endif
using UnityEngine;

namespace BUCK.BestFitText
{
    /// <inheritdoc />
    /// <summary>
    /// Automatically resize child Text Components when the GameObject this component is attached to is enabled, started and when the screen resolution is changed.
    /// </summary>
    [AddComponentMenu("Layout/Best Fit Automatic"), DisallowMultipleComponent]
    public class BestFitAutomatic : BestFit
    {
        /// <summary>
        /// Should inactive GameObjects and Text Components be resized and used in resizing calculations?
        /// </summary>
        [SerializeField, Tooltip("Should inactive GameObjects and Text Components be resized and used in resizing calculations?")]
        protected bool _includeInactive = true;
        /// <summary>
        /// A list of strings that will also be tested on every Text Component as well as the current Text.
        /// </summary>
        [SerializeField, Tooltip("A list of strings that will also be tested on every Text Component as well as the current Text.")]
        protected List<string> _newStrings = new List<string>();

        protected virtual void Start()
        {
            OnChange();
        }

        protected virtual void OnEnable()
        {
            OnChange();
            ResolutionChange += OnChange;
        }

        protected virtual void OnDisable()
        {
            ResolutionChange -= OnChange;
        }

        [ContextMenu("Best Fit Child Text")]
        public virtual void OnChange()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Undo.RegisterCompleteObjectUndo(GetComponentsInChildren<MaskableGraphic>(true), "Best Fit Child Text");
            }
#endif
            OnChange(_includeInactive, _newStrings);
        }

        /// <summary>
        /// Update the size of the Text Components which are affected by this Component.
        /// </summary>
        /// <param name="includeInactive">Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        public void OnChange(bool includeInactive, List<string> newStrings = null)
        {
            gameObject.BestFit(includeInactive, newStrings);
        }
    }
}