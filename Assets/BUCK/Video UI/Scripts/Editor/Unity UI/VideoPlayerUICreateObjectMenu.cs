using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BUCK.VideoUI.UnityUI.Editor
{
    public static class VideoPlayerUICreateObjectMenu
    {
        /// <summary>
        /// Create a VideoPlayerUI GameObject.
        /// </summary>
        [MenuItem("GameObject/Video/Video Player UI", false, 2003)]
        private static void CreateVideoPlayerUIObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Video Player UI", typeof(VideoPlayerUI), typeof(VideoPlayer), typeof(RawImage), typeof(AudioSource), typeof(AspectRatioFitter));
            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject, parentObject != null ? parentObject.scene : gameObject.scene);
            if (gameObject.transform.parent == null && parentObject != null)
            {
                Undo.SetTransformParent(gameObject.transform, parentObject.transform, "Parent " + gameObject.name);
            }
            Undo.SetCurrentGroupName("Create " + gameObject.name);
            GameObjectUtility.SetParentAndAlign(gameObject, parentObject);
            gameObject.GetComponent<VideoPlayerUI>().GetVideoPlayer();
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeGameObject = gameObject;
        }
    }
}