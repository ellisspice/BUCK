#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BUCK.LoadingUI
{
    /// <summary>
    /// Static Loading UI Manager.
    /// </summary>
    public static class Loading
    {
        /// <summary>
        /// Loading UI within the current scene.
        /// </summary>
        private static BaseLoadingUI _loadingUi { get; set; }

        /// <summary>
        /// Is the Loading UI currently being displayed?
        /// </summary>
        public static bool IsActive => _loadingUi != null && _loadingUi.isActiveAndEnabled;

        /// <summary>
        /// Constructor.
        /// </summary>
        static Loading()
        {
            SceneManager.activeSceneChanged += GetLoadingUI;
            GetLoadingUI();
        }

        /// <summary>
        /// Get the current Loading UI object, triggered by a change of scene.
        /// </summary>
        private static void GetLoadingUI(Scene arg0, Scene arg1)
        {
            GetLoadingUI();
        }

        /// <summary>
        /// Get the current Loading UI object.
        /// </summary>
        private static void GetLoadingUI()
        {
            SetLoadingUI(Resources.FindObjectsOfTypeAll<BaseLoadingUI>().FirstOrDefault(l => l.gameObject.scene == SceneManager.GetActiveScene()));
        }

        /// <summary>
        /// Set the current Loading UI.
        /// </summary>
        /// <param name="ui">BaseLoadingUI in the current scene.</param>
        private static void SetLoadingUI(BaseLoadingUI ui)
        {
            _loadingUi = ui;
            if (_loadingUi)
            {
                _loadingUi.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Set the value related to loading for the current Loading UI.
        /// </summary>
        /// <param name="value">Optional. Value to set for the current Loading UI.</param>
        public static void SetValue(float value = 0)
        {
            if (_loadingUi)
            {
                _loadingUi.SetValue(value);
            }
        }
        
        /// <summary>
        /// Set the displayed loading text.
        /// </summary>
        /// <param name="text">Text to display in the Loading UI.</param>
        public static void SetText(string text)
        {
            if (_loadingUi)
            {
                _loadingUi.SetText(text);
            }
        }

        /// <summary>
        /// Display the Loading UI.
        /// </summary>
        /// <param name="text">Optional. Text to display in the Loading UI.</param>
        public static void Start(string text = "")
        {
            if (_loadingUi)
            {
                _loadingUi.StartLoading(text);
            }
        }

        /// <summary>
        /// Stop displaying the Loading UI after stopDelay seconds.
        /// </summary>
        /// <param name="text">Optional. Text to display in the Loading UI.</param>
        /// <param name="stopDelay">Optional. Number of seconds before the Loading UI is hidden.</param>
        public static void Stop(string text = "", float stopDelay = 0f)
        {
            if (_loadingUi)
            {
                _loadingUi.StopLoading(text, stopDelay);
            }
        }
    }
}