using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BUCK.VideoUI.OldInputSystem.Editor
{
    public static class AutoVideoPlayerCreateObjectMenu
    {
        /// <summary>
        /// Create a AutoVideoPlayer GameObject.
        /// </summary>
        [MenuItem("GameObject/Video/Auto Video Player", false, 2001)]
        private static void CreateAutoVideoPlayerObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Auto Video Player", typeof(AutoVideoPlayer), typeof(VideoPlayer), typeof(RawImage), typeof(AudioSource), typeof(AspectRatioFitter), typeof(CanvasGroup));
            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject, parentObject != null ? parentObject.scene : gameObject.scene);
            if (gameObject.transform.parent == null && parentObject != null)
            {
                Undo.SetTransformParent(gameObject.transform, parentObject.transform, "Parent " + gameObject.name);
            }
            Undo.SetCurrentGroupName("Create " + gameObject.name);
            GameObjectUtility.SetParentAndAlign(gameObject, parentObject);
            gameObject.GetComponent<AutoVideoPlayer>().GetVideoPlayer();
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeGameObject = gameObject;
        }
    }
}