using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BUCK.MenuSystem.SelectionMaintainer.UnityUI.Editor
{
    public static class SMInputFieldCreateObjectMenu
    {
        /// <summary>
        /// Create a Input Field GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Input Field (Selection Maintainer)", false, 2127)]
        private static void CreateSMButtonObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Input Field", typeof(Image), typeof(SMInputField));
            gameObject.GetComponent<SMInputField>().targetGraphic = gameObject.GetComponent<Image>();
            var placeholderObject = new GameObject("Placeholder", typeof(Text));
            GameObjectUtility.SetParentAndAlign(placeholderObject, gameObject);
            var placeholderRectTransform = placeholderObject.GetComponent<RectTransform>();
            placeholderRectTransform.anchorMin = Vector2.zero;
            placeholderRectTransform.anchorMax = Vector2.one;
            placeholderRectTransform.sizeDelta = Vector2.zero;
            placeholderRectTransform.anchoredPosition = Vector2.zero;
            var placeholderText = placeholderObject.GetComponent<Text>();
            placeholderText.fontStyle = FontStyle.Italic;
            placeholderText.text = "Enter text...";
            placeholderText.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            gameObject.GetComponent<SMInputField>().placeholder = placeholderText;
            var textObject = new GameObject("Text", typeof(Text));
            GameObjectUtility.SetParentAndAlign(textObject, gameObject);
            var textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.anchoredPosition = Vector2.zero;
            textObject.GetComponent<Text>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
            gameObject.GetComponent<SMInputField>().textComponent = textObject.GetComponent<Text>();
            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject, parentObject != null ? parentObject.scene : gameObject.scene);
            if (gameObject.transform.parent == null && parentObject != null)
            {
                Undo.SetTransformParent(gameObject.transform, parentObject.transform, "Parent " + gameObject.name);
            }
            Undo.SetCurrentGroupName("Create " + gameObject.name);
            GameObjectUtility.SetParentAndAlign(gameObject, parentObject);
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeGameObject = gameObject;
        }
    }
}