using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BUCK.LoadingUI.UnityUI.Editor
{
    public static class LoadingSpinnerCreateObjectMenu
    {
        /// <summary>
        /// Create a LoadingSpinner GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Loading Spinner", false, 2133)]
        private static void CreateLoadingSpinnerObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Loading Spinner", typeof(LoadingSpinner));
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
            var spinnerObject = new GameObject("Spinner", typeof(Image));
            GameObjectUtility.SetParentAndAlign(spinnerObject, gameObject);
            var spinnerRectTransform = spinnerObject.GetComponent<RectTransform>();
            spinnerRectTransform.anchorMin = new Vector2(0.35f, 0.25f);
            spinnerRectTransform.anchorMax = new Vector2(0.65f, 0.75f);
            spinnerRectTransform.sizeDelta = Vector2.zero;
            spinnerRectTransform.anchoredPosition = Vector2.zero;
            var textObject = new GameObject("Text", typeof(Text));
            GameObjectUtility.SetParentAndAlign(textObject, gameObject);
            var textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = new Vector2(0.25f, 0.05f);
            textRectTransform.anchorMax = new Vector2(0.75f, 0.2f);
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.anchoredPosition = Vector2.zero;
            var textComponent = textObject.GetComponent<Text>();
            textComponent.resizeTextForBestFit = true;
            textComponent.alignment = TextAnchor.MiddleCenter;
            gameObject.GetComponent<LoadingSpinner>().SetUIObjects(textComponent, spinnerObject.GetComponent<Image>(), 15);
                
            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject, parentObject != null ? parentObject.scene : gameObject.scene);
            if (gameObject.transform.parent == null && parentObject != null)
            {
                Undo.SetTransformParent(gameObject.transform, parentObject.transform, "Parent " + gameObject.name);
            }
            Undo.SetCurrentGroupName("Create " + gameObject.name);
            GameObjectUtility.SetParentAndAlign(gameObject, parentObject);
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeGameObject = gameObject;
        }
    }
}