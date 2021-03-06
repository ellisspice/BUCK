﻿#if NEW_INPUT_SYSTEM_INCLUDED
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.MenuSystem.MenuController.NewInputSystem.Editor
{
    public static class MenuBehaviourCreateObjectMenu
    {
        /// <summary>
        /// Create a MenuBehaviour GameObject.
        /// </summary>
        [MenuItem("GameObject/UI/Menu Behaviour (New Input System)", false, 2125)]
        private static void CreateMenuBehaviourObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Menu Object", typeof(MenuBehaviour));
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