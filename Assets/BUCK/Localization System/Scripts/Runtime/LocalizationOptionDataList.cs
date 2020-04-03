using System;
using System.Collections.Generic;
using UnityEngine;

namespace BUCK.LocalizationSystem
{
    /// <summary>
    /// Class used internally to store the list of options for the dropdown list.
    /// </summary>
    /// <remarks>
    /// The usage of this class is not exposed in the runtime API. It's only relevant for the PropertyDrawer drawing the list of options.
    /// </remarks>
    [Serializable]
    public class LocalizationOptionDataList
    {
        [SerializeField]
        protected List<LocalizationOptionData> _options;

        /// <summary>
        /// The list of options for the dropdown list.
        /// </summary>
        public List<LocalizationOptionData> Options { get => _options; set => _options = value; }
        
        public LocalizationOptionDataList()
        {
            Options = new List<LocalizationOptionData>();
        }
    }
}