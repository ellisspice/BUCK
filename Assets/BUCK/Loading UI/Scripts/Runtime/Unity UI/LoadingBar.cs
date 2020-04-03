using UnityEngine;
using UnityEngine.UI;

namespace BUCK.LoadingUI.UnityUI
{
    /// <inheritdoc />
    /// <summary>
    /// Class used to display an image which fills based on set values.
    /// </summary>
    [AddComponentMenu("UI/Loading Bar"), RequireComponent(typeof(Image))]
    public class LoadingBar : BaseLoadingUI
    {
        /// <summary>
        /// Image which will be filled as the loading value increases.
        /// </summary>
        [SerializeField, Tooltip("Image which will be filled as the loading value increases.")]
        protected Image _loadingBar;

        /// <inheritdoc />
        /// <summary>
        /// Set the fill amount of the image.
        /// </summary>
        public override void SetValue(float value = 0)
        {
            _loadingBar.fillAmount = value;
        }

        /// <inheritdoc />
        public override void StartLoading(string text)
        {
            base.StartLoading(text);
            _loadingBar.fillAmount = 0;
        }

        /// <inheritdoc />
        public override void StopLoading(string text, float stopDelay)
        {
            base.StopLoading(text, stopDelay);
            _loadingBar.fillAmount = 1;
        }
        
        /// <summary>
        /// Set the UI objects used within this Loading UI.
        /// Primary use is to creating Loading UI objects in the editor.
        /// </summary>
        /// <param name="text">Loading UI Text.</param>
        /// <param name="bar">Loading UI image which will fill.</param>
        public virtual void SetUIObjects(Text text, Image bar)
        {
            _text = text;
            _loadingBar = bar;
        }
    }
}