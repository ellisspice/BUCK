#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using UnityEngine;

namespace BUCK.LocalizationSystem
{
    /// <inheritdoc />
    /// <summary>
    /// Generic class for creating a Localization UI Component.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class UILocalization : MonoBehaviour
    {
        #region LocalizationTesting
        /// <summary>
        /// Name of the CultureInfo to use during in-editor testing.
        /// </summary>
        [HideInInspector]
        public string LanguageOverride;
        #endregion

        protected virtual void OnEnable()
        {
            Set();
        }

        /// <summary>
        /// Update the associated UI Component on this GameObject.
        /// </summary>
        public abstract void Set();
    }
}