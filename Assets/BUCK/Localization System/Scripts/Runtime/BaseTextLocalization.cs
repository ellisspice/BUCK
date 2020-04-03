#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BUCK.LocalizationSystem
{
    /// <inheritdoc />
    /// <summary>
    /// Class for setting the text displayed on a Text Component using the values provided.
    /// </summary>
    public abstract class BaseTextLocalization : UILocalization
    {
        /// <summary>
        /// The localization key.
        /// </summary>
        [SerializeField, Tooltip("The localization key.")]
        protected string _key;
        /// <summary>
        /// Should the text be made uppercase?
        /// </summary>
        [SerializeField, Tooltip("Should the text be made uppercase?")]
        protected bool _toUpper;
        /// <summary>
        /// Args to be inserted into the localization string.
        /// </summary>
        [SerializeField, Tooltip("Args to be inserted into the localization string.")]
        protected string[] _formatArgs;

        /// <summary>
        /// The localization key.
        /// </summary>
        public string Key { get => _key; set { _key = value; Set(); } }
        /// <summary>
        /// Should the text be made uppercase?
        /// </summary>
        public bool ToUpper { get => _toUpper; set { _toUpper = value; Set(); } }
        /// <summary>
        /// Args to be inserted into the localization string.
        /// </summary>
        public string[] FormatArgs => _formatArgs;

        /// <summary>
        /// Update localization values.
        /// </summary>
        /// <param name="key">Localization key to use.</param>
        /// <param name="toUpper">Should the localized text be made uppercase?</param>
        /// <param name="args">Args (if any) to insert into the localized string using string.Format.</param>
        public virtual void Set(string key, bool toUpper, params object[] args)
        {
            _key = key;
            _toUpper = toUpper;
            _formatArgs = args.Select(a => Convert.ToString(a, Localization.SpecificSelectedLanguage)).ToArray();
            Set();
        }

        /// <summary>
        /// Update the args that are inserted into the localized string.
        /// </summary>
        /// <param name="args">Args (if any) to insert into the localized string using string.Format.</param>
        public virtual void SetArgs(params object[] args)
        {
            Set(_key, _toUpper, args);
        }

        /// <summary>
        /// Update a single arg that will be inserted into the localized string.
        /// </summary>
        /// <param name="index">Index in args to be updated.</param>
        /// <param name="arg">Arg to insert into the localized string using string.Format.</param>
        public virtual void SetArg(int index, object arg)
        {
            var argString = Convert.ToString(arg, Localization.SpecificSelectedLanguage);
            if (index < _formatArgs.Length)
            {
                _formatArgs[index] = argString;
            }
            else
            {
                var argsList = new List<string>();
                for (var i = 0; i < index; i++)
                {
                    argsList.Add(i < _formatArgs.Length ? _formatArgs[i] : string.Empty);
                    argsList.Add(argString);
                }
                _formatArgs = argsList.ToArray();
            }
            Set();
        }
    }
}