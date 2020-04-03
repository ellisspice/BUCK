#if TEXT_MESH_PRO_INCLUDED
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BUCK.MenuSystem.SelectionMaintainer.TMPro.Editor
{
    public static class SMInputFieldCreateObjectMenu
    {
        /// <summary>
        /// Create a Input Field GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Input Field (Selection Maintainer, Text Mesh Pro)", false, 2128)]
        private static void CreateSMButtonObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Input Field", typeof(Image), typeof(SMInputField));
            gameObject.GetComponent<SMInputField>().targetGraphic = gameObject.GetComponent<Image>();
            var textAreaObject = new GameObject("Text Area", typeof(RectMask2D));
            var textAreaRectTransform = textAreaObject.GetComponent<RectTransform>();
            textAreaRectTransform.anchorMin = Vector2.zero;
            textAreaRectTransform.anchorMax = Vector2.one;
            textAreaRectTransform.sizeDelta = Vector2.zero;
            textAreaRectTransform.anchoredPosition = Vector2.zero;
            gameObject.GetComponent<SMInputField>().textViewport = textAreaRectTransform;
            GameObjectUtility.SetParentAndAlign(textAreaObject, gameObject);
            var placeholderObject = new GameObject("Placeholder", typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(placeholderObject, textAreaObject);
            var placeholderRectTransform = placeholderObject.GetComponent<RectTransform>();
            placeholderRectTransform.anchorMin = Vector2.zero;
            placeholderRectTransform.anchorMax = Vector2.one;
            placeholderRectTransform.sizeDelta = Vector2.zero;
            placeholderRectTransform.anchoredPosition = Vector2.zero;
            var placeholderText = placeholderObject.GetComponent<TextMeshProUGUI>();
            placeholderText.fontStyle = FontStyles.Italic;
            placeholderText.text = "Enter text...";
            placeholderText.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            gameObject.GetComponent<SMInputField>().placeholder = placeholderText;
            var textObject = new GameObject("Text", typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(textObject, textAreaObject);
            var textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.anchoredPosition = Vector2.zero;
            textObject.GetComponent<TextMeshProUGUI>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
            gameObject.GetComponent<SMInputField>().textComponent = textObject.GetComponent<TextMeshProUGUI>();
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
#endif