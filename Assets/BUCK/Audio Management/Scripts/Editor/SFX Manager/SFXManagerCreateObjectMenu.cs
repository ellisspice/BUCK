using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.AudioManagement.SFX.Editor
{
    public static class SfxManagerCreateObjectMenu
    {
        /// <summary>
        /// Create a SFXManager GameObject.
        /// </summary>
        [MenuItem("GameObject/Audio/SFX Manager", false, 2001)]
        private static void CreateSFXManagerObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Sfx Manager", typeof(SFXManager));
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