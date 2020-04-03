using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.MenuSystem.MenuController.Editor
{
    public static class MenuManagerCreateObjectMenu
    {
        /// <summary>
        /// Create a MenuManager GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Menu Manager", false, 2123)]
        private static void CreateMenuManagerObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Menu Manager", typeof(MenuManager));
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