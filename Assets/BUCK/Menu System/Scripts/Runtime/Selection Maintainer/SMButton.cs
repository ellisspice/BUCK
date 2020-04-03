using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BUCK.MenuSystem.SelectionMaintainer
{
    /// <summary>
    /// Button class to use in order to keep UI selection at all times.
    /// </summary>
    [AddComponentMenu("UI/Button (Selection Maintainer)")]
    public class SMButton : Button
    {
        /// <summary>
        /// Determine in which of the 4 move directions the next selectable object should be found.
        /// The same code as found within Unity's provided Selectable, duplicated in order to call edited Navigate method.
        /// </summary>
        public override void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Right:
                    Navigate(eventData, FindSelectableOnRight());
                    break;
                case MoveDirection.Up:
                    Navigate(eventData, FindSelectableOnUp());
                    break;
                case MoveDirection.Left:
                    Navigate(eventData, FindSelectableOnLeft());
                    break;
                case MoveDirection.Down:
                    Navigate(eventData, FindSelectableOnDown());
                    break;
                case MoveDirection.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Change the selection to the specified object if it is not null.
        /// Different from default functionality, which will also not select if not active.
        /// </summary>
        protected virtual void Navigate(AxisEventData eventData, Selectable sel)
        {
            if (sel)
            {
                eventData.selectedObject = sel.gameObject;
            } 
        }
    }
}