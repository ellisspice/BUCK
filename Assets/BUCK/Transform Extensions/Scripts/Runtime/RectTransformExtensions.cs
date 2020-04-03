#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using UnityEngine;

namespace BUCK.TransformExtensions
{
    /// <summary>
    /// Set of extensions to get RectTransform.
    /// </summary>
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Get the RectTransform for the Transform provided.
        /// </summary>
        /// <param name="transform">Transform for which the RectTransform will be returned.</param>
        /// <returns>RectTransform for the provided Transform.</returns>
        public static RectTransform RectTransform(this Transform transform)
        {
            return (RectTransform)transform;
        }

        /// <summary>
        /// Get the RectTransform for the GameObject provided.
        /// </summary>
        /// <param name="gameObject">GameObject for which the RectTransform will be returned.</param>
        /// <returns>RectTransform for the provided GameObject.</returns>
        public static RectTransform RectTransform(this GameObject gameObject)
        {
            return gameObject.transform.RectTransform();
        }

        /// <summary>
        /// Get the RectTransform for the Component provided.
        /// </summary>
        /// <param name="com">Component for which the RectTransform will be returned.</param>
        /// <returns>RectTransform for the provided Component.</returns>
        public static RectTransform RectTransform<T>(this T com) where T : Component
        {
            return com.transform.RectTransform();
        }

        /// <summary>
        /// Get the RectTransform for the child whose name matches the string provided.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>RectTransform for the child whose name matches.</returns>
        public static RectTransform FindRect(this Transform transform, string find)
        {
            transform.RectTransform();
            return transform.Find(find).RectTransform();
        }
    }
}