using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BUCK.LoadingUI.UnityUI.Editor
{
    public static class LoadingBarCreateObjectMenu
    {
        /// <summary>
        /// Create a LoadingBar GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Loading Bar", false, 2131)]
        private static void CreateLoadingBarObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Loading Bar", typeof(LoadingBar));
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
            var backgroundObject = new GameObject("Background", typeof(Image));
            backgroundObject.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
            GameObjectUtility.SetParentAndAlign(backgroundObject, gameObject);
            var backgroundRectTransform = backgroundObject.GetComponent<RectTransform>();
            backgroundRectTransform.anchorMin = new Vector2(0.25f, 0.4f);
            backgroundRectTransform.anchorMax = new Vector2(0.75f, 0.6f);
            backgroundRectTransform.sizeDelta = Vector2.zero;
            backgroundRectTransform.anchoredPosition = Vector2.zero;
            var barObject = new GameObject("Bar", typeof(Image));
            GameObjectUtility.SetParentAndAlign(barObject, backgroundObject);
            var barImage = barObject.GetComponent<Image>();
            barImage.type = Image.Type.Filled;
            barImage.fillMethod = Image.FillMethod.Horizontal;
            barImage.fillAmount = 0;
            var barRectTransform = barObject.GetComponent<RectTransform>();
            barRectTransform.anchorMin = Vector2.zero;
            barRectTransform.anchorMax = Vector2.one;
            barRectTransform.sizeDelta = new Vector2(-10, -10);
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
            gameObject.GetComponent<LoadingBar>().SetUIObjects(textComponent, barImage);

            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject,
                parentObject != null ? parentObject.scene : gameObject.scene);
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