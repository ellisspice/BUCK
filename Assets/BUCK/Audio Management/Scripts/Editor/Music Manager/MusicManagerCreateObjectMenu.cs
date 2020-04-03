using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.AudioManagement.Music.Editor
{
    public static class MusicManagerCreateObjectMenu
    {
        /// <summary>
        /// Create a MusicManager GameObject.
        /// </summary>
        [MenuItem("GameObject/Audio/Music Manager", false, 2000)]
        private static void CreateMusicManagerObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Music Manager", typeof(MusicManager));
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