using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BUCK.VideoUI.UnityUI.Editor
{
    public static class VideoPlaylistPlayerUICreateObjectMenu
    {
        /// <summary>
        /// Create a VideoPlaylistPlayerUI GameObject.
        /// </summary>
        [MenuItem("GameObject/Video/Video Playlist Player UI", false, 2005)]
        private static void CreateVideoPlaylistPlayerUIObject(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Video Playlist Player UI", typeof(VideoPlaylistPlayerUI), typeof(VideoPlayer), typeof(RawImage), typeof(AudioSource), typeof(AspectRatioFitter));
            var parentObject = menuCommand.context as GameObject;
            SceneManager.MoveGameObjectToScene(gameObject, parentObject != null ? parentObject.scene : gameObject.scene);
            if (gameObject.transform.parent == null && parentObject != null)
            {
                Undo.SetTransformParent(gameObject.transform, parentObject.transform, "Parent " + gameObject.name);
            }
            Undo.SetCurrentGroupName("Create " + gameObject.name);
            GameObjectUtility.SetParentAndAlign(gameObject, parentObject);
            gameObject.GetComponent<VideoPlaylistPlayerUI>().GetVideoPlayer();
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeGameObject = gameObject;
        }
    }
}