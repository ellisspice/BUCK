using System;
using UnityEngine;

namespace BUCK.MenuSystem.Orientation
{
    /// <summary>
    /// Object for setting min and max anchor values.
    /// </summary>
    [Serializable]
    public struct UIItemAnchors
    {
        public Vector2 MinAnchors;
        public Vector2 MaxAnchors;
    }
}