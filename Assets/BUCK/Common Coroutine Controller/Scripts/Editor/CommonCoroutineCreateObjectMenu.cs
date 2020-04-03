using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.CommonCoroutineController.Editor
{
    public static class CommonCoroutineCreateObjectMenu
    {
        /// <summary>
        /// Create a CommonCoroutine GameObject.
        /// </summary>
        [MenuItem("GameObject/Managers/Common Coroutine Controller", false, 21)]
        private static void CreateCommonCoroutineObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Common Coroutine Controller", typeof(CommonCoroutine));
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