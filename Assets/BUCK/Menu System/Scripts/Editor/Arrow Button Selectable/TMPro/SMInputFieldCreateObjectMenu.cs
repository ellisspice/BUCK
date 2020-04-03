#if TEXT_MESH_PRO_INCLUDED
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BUCK.MenuSystem.ArrowButtons.TMPro.Editor
{
    public static class ArrowButtonSelectableCreateObjectMenu
    {
        /// <summary>
        /// Create an ArrowButtonSelectable GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Arrow Button Selectable (Text Mesh Pro)", false, 2122)]
        private static void CreateArrowButtonSelectableObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Arrow Button Selectable", typeof(Image), typeof(ArrowButtonSelectable));
            gameObject.GetComponent<ArrowButtonSelectable>().targetGraphic = gameObject.GetComponent<Image>();
            var leftButtonObject = new GameObject("Left", typeof(Image), typeof(Button));
            leftButtonObject.GetComponent<Button>().targetGraphic = leftButtonObject.GetComponent<Image>();
            GameObjectUtility.SetParentAndAlign(leftButtonObject, gameObject);
            var leftRectTransform = leftButtonObject.GetComponent<RectTransform>();
            leftRectTransform.anchorMin = Vector2.zero;
            leftRectTransform.anchorMax = new Vector2(0.2f, 1f);
            leftRectTransform.sizeDelta = Vector2.zero;
            leftRectTransform.anchoredPosition = Vector2.zero;
            gameObject.GetComponent<ArrowButtonSelectable>().LeftButton = leftButtonObject.GetComponent<Button>();
            var leftTextObject = new GameObject("Text", typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(leftTextObject, leftButtonObject);
            var leftTextRectTransform = leftTextObject.GetComponent<RectTransform>();
            leftTextRectTransform.anchorMin = Vector2.zero;
            leftTextRectTransform.anchorMax = Vector2.one;
            leftTextRectTransform.sizeDelta = Vector2.zero;
            leftTextRectTransform.anchoredPosition = Vector2.zero;
            leftTextObject.GetComponent<TextMeshProUGUI>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
            leftTextObject.GetComponent<TextMeshProUGUI>().text = "<";
            leftTextObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            var textObject = new GameObject("Text", typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(textObject, gameObject);
            var textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = new Vector2(0.2f, 0f);
            textRectTransform.anchorMax = new Vector2(0.8f, 1f);
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.anchoredPosition = Vector2.zero;
            textObject.GetComponent<TextMeshProUGUI>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
            textObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            gameObject.GetComponent<ArrowButtonSelectable>().SelectedText = textObject.GetComponent<TextMeshProUGUI>();
            var rightButtonObject = new GameObject("Right", typeof(Image), typeof(Button));
            rightButtonObject.GetComponent<Button>().targetGraphic = rightButtonObject.GetComponent<Image>();
            GameObjectUtility.SetParentAndAlign(rightButtonObject, gameObject);
            var rightRectTransform = rightButtonObject.GetComponent<RectTransform>();
            rightRectTransform.anchorMin = new Vector2(0.8f, 0f);
            rightRectTransform.anchorMax = Vector2.one;
            rightRectTransform.sizeDelta = Vector2.zero;
            rightRectTransform.anchoredPosition = Vector2.zero;
            gameObject.GetComponent<ArrowButtonSelectable>().RightButton = rightButtonObject.GetComponent<Button>();
            var rightTextObject = new GameObject("Text", typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(rightTextObject, rightButtonObject);
            var rightTextRectTransform = rightTextObject.GetComponent<RectTransform>();
            rightTextRectTransform.anchorMin = Vector2.zero;
            rightTextRectTransform.anchorMax = Vector2.one;
            rightTextRectTransform.sizeDelta = Vector2.zero;
            rightTextRectTransform.anchoredPosition = Vector2.zero;
            rightTextObject.GetComponent<TextMeshProUGUI>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
            rightTextObject.GetComponent<TextMeshProUGUI>().text = ">";
            rightTextObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject, parentObject != null ? parentObject.scene : gameObject.scene);
            if (gameObject.transform.parent == null && parentObject != null)
            {
                Undo.SetTransformParent(gameObject.transform, parentObject.transform, "Parent " + gameObject.name);
            }
            Undo.SetCurrentGroupName("Create " + gameObject.name);
            GameObjectUtility.SetParentAndAlign(gameObject, parentObject);
            var goRectTransform = gameObject.GetComponent<RectTransform>();
            goRectTransform.anchorMin = Vector2.zero;
            goRectTransform.anchorMax = Vector2.one;
            goRectTransform.sizeDelta = Vector2.zero;
            goRectTransform.anchoredPosition = Vector2.zero;
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeGameObject = gameObject;
        }
    }
}
#endif