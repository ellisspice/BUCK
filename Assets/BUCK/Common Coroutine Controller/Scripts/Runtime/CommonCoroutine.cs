using System.Collections;
using UnityEngine;

namespace BUCK.CommonCoroutineController
{
    /// <inheritdoc />
    /// <summary>
    /// A singleton which can be used to call coroutines in non-MonoBehaviours.
    /// </summary>
    [AddComponentMenu("Managers/Common Coroutine")]
    public class CommonCoroutine : MonoBehaviour
    {
        /// <summary>
        /// Singleton.
        /// </summary>
        protected static CommonCoroutine _instance;

        protected virtual void Awake()
        {
            //Destroy this GameObject if a singleton already exists.
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            //Otherwise set this version of the Component as the singleton and set it to not be destroyed when changing scenes.
            _instance = this;
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Start a Coroutine within the singleton for the IEnumerator provided.
        /// </summary>
        /// <param name="routine">IEnumerator to start Coroutine for.</param>
        /// <returns>Returns Coroutine created for the IEnumerator.</returns>  
        public static Coroutine StartStaticCoroutine(IEnumerator routine)
        {
            return _instance.StartCoroutine(routine);
        }

        /// <summary>
        /// Stop the Coroutine within the singleton for the IEnumerator provided.
        /// </summary>
        /// <param name="routine">IEnumerator to stop Coroutine for.</param>
        public static void StopStaticCoroutine(IEnumerator routine)
        {
            _instance.StopCoroutine(routine);
        }
        
        /// <summary>
        /// Stop the Coroutine provided within the singleton.
        /// </summary>
        /// <param name="routine">Coroutine to stop.</param>
        public static void StopStaticCoroutine(Coroutine routine)
        {
            _instance.StopCoroutine(routine);
        }

        /// <summary>
        /// Stop all Coroutines within the singleton.
        /// </summary>
        public static void StopAllStaticCoroutines()
        {
            _instance.StopAllCoroutines();
        }
    }
}