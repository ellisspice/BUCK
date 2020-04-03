#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.LoadingUI
{
    /// <inheritdoc />
    /// <summary>
    /// Class used to display a UI related to loading.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BaseLoadingUI : MonoBehaviour
    {
        /// <summary>
        /// WaitForEndOfFrame used for Coroutines.
        /// </summary>
        protected readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        /// <summary>
        /// Set the value related to loading for this Loading UI.
        /// </summary>
        /// <param name="value">Optional. Value to set for this Loading UI.</param>
        public abstract void SetValue(float value = 0);

        /// <summary>
        /// Set the displayed loading text.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public abstract void SetText(string text);

        /// <summary>
        /// Display this Loading UI.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public virtual void StartLoading(string text)
        {
            StopAllCoroutines();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Stop displaying the Loading UI after stopDelay seconds.
        /// </summary>
        /// <param name="text">Text to display in the Loading UI.</param>
        /// <param name="stopDelay">Number of seconds before the Loading UI is hidden.</param>
        public virtual void StopLoading(string text, float stopDelay)
        {
            StartCoroutine(Stop(stopDelay));
        }

        /// <summary>
        /// Stop displaying the Loading UI.
        /// </summary>
        /// <param name="stopDelay">Optional. Number of seconds before the Loading UI is hidden.</param>
        protected virtual IEnumerator Stop(float stopDelay = 0)
        {
            var timer = 0f;
            while (timer < stopDelay)
            {
                timer += Time.smoothDeltaTime;
                yield return _waitForEndOfFrame;
            }
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
    }
    
    /// <inheritdoc />
    public abstract class BaseLoadingUI<T> : BaseLoadingUI where T : MaskableGraphic
    {
        /// <summary>
        /// Optional Text component displayed during loading.
        /// </summary>
        [SerializeField, Tooltip("Optional Text component displayed during loading.")]
        protected T _text;
    }
}