#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using UnityEngine;
using UnityEngine.UI;
#if TEXT_MESH_PRO_INCLUDED
using TMPro;
#endif

namespace BUCK.TransformExtensions
{
    /// <summary>
    /// Set of extensions related to the Transform component.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Find a child of the Transform using the string provided and return component T of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child component which matches criteria provided.</returns>
        public static T FindComponent<T>(this Transform transform, string find) where T : Component
        {
            var compTrans = transform.Find(find);
            return compTrans != null ? compTrans.GetComponent<T>() : null;
        }

        /// <summary>
        /// Find a child of the Transform using the string provided and return the Image component of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Image component whose name matches.</returns>
        public static Image FindImage(this Transform transform, string find)
        {
            return transform.FindComponent<Image>(find);
        }

#if TEXT_MESH_PRO_INCLUDED
        /// <summary>
        /// Find a child of the Transform using the string provided and return the Text component of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child TextMeshProUGUI component whose name matches.</returns>
        public static TextMeshProUGUI FindTMProText(this Transform transform, string find)
        {
            return transform.FindComponent<TextMeshProUGUI>(find);
        }
#endif
        
        /// <summary>
        /// Find a child of the Transform using the string provided and return the Text component of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Text component whose name matches.</returns>
        public static Text FindText(this Transform transform, string find)
        {
            return transform.FindComponent<Text>(find);
        }


        /// <summary>
        /// Find a child of the Transform using the string provided and return the Button component of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Button component whose name matches.</returns>
        public static Button FindButton(this Transform transform, string find)
        {
            return transform.FindComponent<Button>(find);
        }

        /// <summary>
        /// Find a child of the Transform using the string provided and return the GameObject of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child GameObject whose name matches.</returns>
        public static GameObject FindObject(this Transform transform, string find)
        {
            return transform.Find(find).gameObject;
        }

        /// <summary>
        /// Return the parent GameObject of the Transform provided.
        /// </summary>
        /// <param name="transform">Transform for which the parent GameObject (if any) is gotten.</param>
        /// <returns>Parent GameObject.</returns>
        public static GameObject Parent(this Transform transform)
        {
            var parent = transform.parent;
            return parent ? parent.gameObject : null;
        }

        /// <summary>
        /// Find a child of the Transform using the string provided and return child component of type T of that child.
        /// </summary>
        /// <param name="transform">Transform the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <param name="includeInactive">Optional. Should Component on inactive GameObjects be included?</param>
        /// <returns>Child component of child which matches name provided.</returns>
        public static T FindComponentInChildren<T>(this Transform transform, string find, bool includeInactive = false) where T : Component
        {
            return transform.Find(find).GetComponentInChildren<T>(includeInactive);
        }

        /// <summary>
        /// Find a child of the GameObject using the string provided and return component T of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child component which matches criteria provided.</returns>
        public static T FindComponent<T>(this GameObject go, string find) where T : Component
        {
            return go.transform.FindComponent<T>(find);
        }

        /// <summary>
        /// Find a child of the GameObject using the string provided and return the Image component of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Image component whose name matches.</returns>
        public static Image FindImage(this GameObject go, string find)
        {
            return go.FindComponent<Image>(find);
        }

        
#if TEXT_MESH_PRO_INCLUDED
        /// <summary>
        /// Find a child of the GameObject using the string provided and return the Text component of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child TextMeshProUGUI component whose name matches.</returns>
        public static TextMeshProUGUI FindTMProText(this GameObject go, string find)
        {
            return go.FindComponent<TextMeshProUGUI>(find);
        }
#endif
        
        /// <summary>
        /// Find a child of the GameObject using the string provided and return the Text component of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Text component whose name matches.</returns>
        public static Text FindText(this GameObject go, string find)
        {
            return go.FindComponent<Text>(find);
        }


        /// <summary>
        /// Find a child of the GameObject using the string provided and return the Button component of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Button component whose name matches.</returns>
        public static Button FindButton(this GameObject go, string find)
        {
            return go.FindComponent<Button>(find);
        }

        /// <summary>
        /// Find a child of the GameObject using the string provided and return the GameObject of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child GameObject whose name matches.</returns>
        public static GameObject FindObject(this GameObject go, string find)
        {
            return go.transform.FindObject(find);
        }

        /// <summary>
        /// Return the parent GameObject of the GameObject provided.
        /// </summary>
        /// <param name="go">GameObject for which the parent GameObject (if any) is gotten.</param>
        /// <returns>Parent GameObject.</returns>
        public static GameObject Parent(this GameObject go)
        {
            return go.transform.Parent();
        }

        /// <summary>
        /// Find a child of the GameObject using the string provided and return child component of type T of that child.
        /// </summary>
        /// <param name="go">GameObject the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <param name="includeInactive">Optional. Should Component on inactive GameObjects be included?</param>
        /// <returns>Child component of child which matches name provided.</returns>
        public static T FindComponentInChildren<T>(this GameObject go, string find, bool includeInactive = false) where T : Component
        {
            return go.transform.FindComponentInChildren<T>(find, includeInactive);
        }

        /// <summary>
        /// Find a child of the Component using the string provided and return component T of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child component whose name matches.</returns>
        public static T FindComponent<T>(this Component com, string find) where T : Component
        {
            return com.transform.FindComponent<T>(find);
        }

        /// <summary>
        /// Find a child of the Component using the string provided and return the Image component of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Image component whose name matches.</returns>
        public static Image FindImage(this Component com, string find)
        {
            return com.FindComponent<Image>(find);
        }

        
#if TEXT_MESH_PRO_INCLUDED
        /// <summary>
        /// Find a child of the Component using the string provided and return the Text component of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child TextMeshProUGUI component whose name matches.</returns>
        public static TextMeshProUGUI FindTMProText(this Component com, string find)
        {
            return com.FindComponent<TextMeshProUGUI>(find);
        }
#endif
        /// <summary>
        /// Find a child of the Component using the string provided and return the Text component of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Text component whose name matches.</returns>
        public static Text FindText(this Component com, string find)
        {
            return com.FindComponent<Text>(find);
        }


        /// <summary>
        /// Find a child of the Component using the string provided and return the Button component of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child Button component whose name matches.</returns>
        public static Button FindButton(this Component com, string find)
        {
            return com.FindComponent<Button>(find);
        }

        /// <summary>
        /// Find a child of the Component using the string provided and return the GameObject of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <returns>Child GameObject whose name matches.</returns>
        public static GameObject FindObject(this Component com, string find)
        {
            return com.transform.FindObject(find);
        }

        /// <summary>
        /// Return the parent GameObject of the Component provided.
        /// </summary>
        /// <param name="com">Component for which the parent GameObject (if any) is gotten.</param>
        /// <returns>Parent GameObject.</returns>
        public static GameObject Parent(this Component com)
        {
            return com.transform.Parent();
        }

        /// <summary>
        /// Find a child of the Component using the string provided and return child component of type T of that child.
        /// </summary>
        /// <param name="com">Component the Find action is performed on.</param>
        /// <param name="find">String used to find a child of the transform provided.</param>
        /// <param name="includeInactive">Optional. Should Component on inactive GameObjects be included?</param>
        /// <returns>Child component of child which matches name provided.</returns>
        public static T FindComponentInChildren<T>(this Component com, string find, bool includeInactive = false) where T : Component
        {
            return com.transform.FindComponentInChildren<T>(find, includeInactive);
        }
    }
}