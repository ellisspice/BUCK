using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BUCK.MenuSystem.SelectionMaintainer.Editor
{
    public static class SMButtonCreateObjectMenu
    {
        /// <summary>
        /// Create a Button GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Button (Selection Maintainer)", false, 2126)]
        private static void CreateSMButtonObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Button", typeof(Image), typeof(SMButton));
            gameObject.GetComponent<SMButton>().targetGraphic = gameObject.GetComponent<Image>();
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