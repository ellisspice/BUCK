using System;

namespace BUCK.MenuSystem.MenuController
{
    /// <summary>
    /// Class used to generate a KeyValuePair of a MenuState and a MenuBehaviour.
    /// </summary>
    [Serializable]
    public class MenuStateObject
    {
        public MenuState MenuState;
        public BaseMenuBehaviour GameObject;
    }
}