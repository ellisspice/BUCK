using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BUCK.MenuSystem.Orientation
{
    /// <inheritdoc />
    /// <summary>
    /// Set anchor values for RectTransforms for landscape and portrait aspect ratios.
    /// </summary>
    [AddComponentMenu("UI/Orientation Anchors"), RequireComponent(typeof(RectTransform))]
    public class OrientationAnchors : MonoBehaviour
    {
        /// <summary>
        /// Event triggered by resolution width or height or camera rect changing.
        /// </summary>
        protected static event Action ResolutionChange = delegate { };
        /// <summary>
        /// Last recorded screen resolution.
        /// </summary>
        protected static Vector2 _previousResolution = Vector2.zero;
        /// <summary>
        /// The RectTransform anchors to use when in a landscape aspect ratio.
        /// </summary>
        [SerializeField, Tooltip("The RectTransform anchors to use when in a landscape aspect ratio.")]
        protected UIItemAnchors _landscapeAnchors;
        /// <summary>
        /// The RectTransform anchors to use when in a portrait aspect ratio.
        /// </summary>
        [SerializeField, Tooltip("The RectTransform anchors to use when in a portrait aspect ratio.")]
        protected UIItemAnchors _portraitAnchors;
        /// <summary>
        /// RectTransform Component on this GameObject.
        /// </summary>
        protected RectTransform _rectTransform;

        protected virtual void Awake()
        {
            if (_previousResolution == Vector2.zero)
            {
                _previousResolution = new Vector2(Screen.width, Screen.height);
            }
            _rectTransform = (RectTransform)transform;
        }

        protected virtual void OnEnable()
        {
            ResolutionChange += UpdateAnchors;
            UpdateAnchors();
        }

        protected virtual void OnDisable()
        {
            ResolutionChange -= UpdateAnchors;
        }

        protected virtual void LateUpdate()
        {
            if (!Mathf.Approximately(_previousResolution.x, Screen.width) || !Mathf.Approximately(_previousResolution.y, Screen.height))
            {
                _previousResolution = new Vector2(Screen.width, Screen.height);
                OnResolutionChange();
            }
        }
        
        /// <summary>
        /// Fire the ResolutionChange event.
        /// </summary>
        public static void OnResolutionChange()
        {
            ResolutionChange?.Invoke();
        }
        
        /// <summary>
        /// Update the RectTransform anchors to match expected values for the current screen resolution.
        /// </summary>
        protected virtual void UpdateAnchors()
        {
            _rectTransform.anchorMin = _previousResolution.x >= _previousResolution.y ? _landscapeAnchors.MinAnchors : _portraitAnchors.MinAnchors;
            _rectTransform.anchorMax = _previousResolution.x >= _previousResolution.y ? _landscapeAnchors.MaxAnchors : _portraitAnchors.MaxAnchors;
            _rectTransform.sizeDelta = Vector2.zero;
            _rectTransform.anchoredPosition = Vector2.zero;
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Set the min anchors for landscape and portrait to match current anchors.
        /// </summary>
        [ContextMenu("Set Min Anchors To Current")]
        protected virtual void SetMinAnchorToCurrent()
        {
            Undo.RegisterCompleteObjectUndo(this, "Set Min Anchors To Current");
            var anchorMin = ((RectTransform)transform).anchorMin;
            _landscapeAnchors.MinAnchors = anchorMin;
            _portraitAnchors.MinAnchors = anchorMin;
        }
        
        /// <summary>
        /// Set the max anchors for landscape and portrait to match current anchors.
        /// </summary>
        [ContextMenu("Set Max Anchors To Current")]
        protected virtual void SetMaxAnchorToCurrent()
        {
            Undo.RegisterCompleteObjectUndo(this, "Set Max Anchors To Current");
            var anchorMax = ((RectTransform)transform).anchorMax;
            _landscapeAnchors.MaxAnchors = anchorMax;
            _portraitAnchors.MaxAnchors = anchorMax;
        }
        
        /// <summary>
        /// Set the min and max anchors for landscape to match current anchors.
        /// </summary>
        [ContextMenu("Set Landscape Anchors To Current")]
        protected virtual void SetLandscapeToCurrent()
        {
            Undo.RegisterCompleteObjectUndo(this, "Set Landscape Anchors To Current");
            var rectTransform = (RectTransform)transform;
            _landscapeAnchors.MinAnchors = rectTransform.anchorMin;
            _landscapeAnchors.MaxAnchors = rectTransform.anchorMax;
        }
        
        /// <summary>
        /// Set the min and max anchors for portrait to match current anchors.
        /// </summary>
        [ContextMenu("Set Portrait Anchors To Current")]
        protected virtual void SetPortraitToCurrent()
        {
            Undo.RegisterCompleteObjectUndo(this, "Set Portrait Anchors To Current");
            var rectTransform = (RectTransform)transform;
            _portraitAnchors.MinAnchors = rectTransform.anchorMin;
            _portraitAnchors.MaxAnchors = rectTransform.anchorMax;
        }
        
        /// <summary>
        /// Set current anchors to match the min and max anchors for landscape.
        /// </summary>
        [ContextMenu("Set Current Anchors To Landscape")]
        protected virtual void SetCurrentToLandscape(bool record = true)
        {
            var rectTransform = (RectTransform)transform;
            if (record)
            {
                Undo.RegisterCompleteObjectUndo(rectTransform, "Set Current Anchors To Landscape");
            }
            rectTransform.anchorMin = _landscapeAnchors.MinAnchors;
            rectTransform.anchorMax = _landscapeAnchors.MaxAnchors;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        /// <summary>
        /// Set current anchors to match the min and max anchors for portrait.
        /// </summary>
        [ContextMenu("Set Current Anchors To Portrait")]
        protected virtual void SetCurrentToPortrait(bool record = true)
        {
            var rectTransform = (RectTransform)transform;
            if (record)
            {
                Undo.RegisterCompleteObjectUndo(rectTransform, "Set Current Anchors To Portrait");
            }
            rectTransform.anchorMin = _portraitAnchors.MinAnchors;
            rectTransform.anchorMax = _portraitAnchors.MaxAnchors;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        /// <summary>
        /// Set all RectTransform with this Component to match landscape values
        /// </summary>
        [MenuItem("Tools/BUCK/Orientation Anchors/Set All To Landscape")]
        protected static void SetAllToLandscape()
        {
            var orientationAnchors = FindObjectsOfType<OrientationAnchors>();
            var rectTransforms = orientationAnchors.Select(o => o.GetComponent<RectTransform>()).ToArray();
            Undo.RegisterCompleteObjectUndo(rectTransforms, "Set All To Landscape");
            foreach (var orientationAnchor in orientationAnchors)
            {
                orientationAnchor.SetCurrentToLandscape(false);
            }
        }
        
        /// <summary>
        /// Set all RectTransform with this Component to match portrait values
        /// </summary>
        [MenuItem("Tools/BUCK/Orientation Anchors/Set All To Portrait")]
        protected static void SetAllToPortrait()
        {
            var orientationAnchors = FindObjectsOfType<OrientationAnchors>();
            var rectTransforms = orientationAnchors.Select(o => o.GetComponent<RectTransform>()).ToArray();
            Undo.RegisterCompleteObjectUndo(rectTransforms, "Set All To Portrait");
            foreach (var orientationAnchor in orientationAnchors)
            {
                orientationAnchor.SetCurrentToPortrait(false);
            }
        }
#endif
    }
}