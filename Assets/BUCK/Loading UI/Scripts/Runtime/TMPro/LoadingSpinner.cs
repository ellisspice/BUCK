#if TEXT_MESH_PRO_INCLUDED
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.LoadingUI.TMPro
{
    /// <inheritdoc />
    /// <summary>
    /// Class used to display a rotating image that indicates loading.
    /// </summary>
    [AddComponentMenu("UI/Loading Spinner (TextMeshPro)"), RequireComponent(typeof(Image))]
    public class LoadingSpinner : BaseLoadingUI
    {
        /// <summary>
        /// Image which will be spun.
        /// </summary>
        [SerializeField, Tooltip("Image which will be spun.")]
        protected Image _spinner; 
        /// <summary>
        /// Number of degrees per second image will be rotated.
        /// </summary>
        [SerializeField, Tooltip("Number of degrees per second image will be rotated.")]
        protected float _spinSpeed;

        /// <inheritdoc />
        /// <summary>
        /// Set the degrees per second image will be rotated.
        /// </summary>
        public override void SetValue(float value = 0)
        {
            _spinSpeed = value;
        }

        /// <inheritdoc />
        public override void StartLoading(string text)
        {
            base.StartLoading(text);
            StartCoroutine(Spin());
        }
        
        /// <summary>
        /// Spin the provided image.
        /// </summary>
        protected virtual IEnumerator Spin()
        {
            while (gameObject.activeInHierarchy)
            {
                _spinner.transform.Rotate(0, 0, -_spinSpeed * Time.smoothDeltaTime);
                yield return _waitForEndOfFrame;
            }
        }

        /// <summary>
        /// Set the UI objects used within this Loading UI.
        /// Primary use is to creating Loading UI objects in the editor.
        /// </summary>
        /// <param name="text">Loading UI Text.</param>
        /// <param name="spinner">Loading UI spinning image.</param>
        /// <param name="speed">Speed to spin image.</param>
        public virtual void SetUIObjects(TextMeshProUGUI text, Image spinner, int speed)
        {
            _text = text;
            _spinner = spinner;
            _spinSpeed = speed;
        }
    }
}
#endif