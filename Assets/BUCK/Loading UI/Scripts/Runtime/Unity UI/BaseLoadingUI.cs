#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using UnityEngine.UI;

namespace BUCK.LoadingUI.UnityUI
{
    /// <inheritdoc />
    public abstract class BaseLoadingUI : BaseLoadingUI<Text>
    {
        /// <inheritdoc />
        public override void SetText(string text)
        {
            if (_text)
            {
                _text.text = text;
            }
        }

        /// <inheritdoc />
        public override void StartLoading(string text)
        {
            base.StartLoading(text);
            if (_text)
            {
                _text.text = text;
            }
        }

        /// <inheritdoc />
        public override void StopLoading(string text, float stopDelay)
        {
            if (_text)
            {
                _text.text = text;
            }
            base.StopLoading(text, stopDelay);
        }
    }
}