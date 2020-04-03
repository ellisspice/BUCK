#if TEXT_MESH_PRO_INCLUDED
using TMPro;

namespace BUCK.LoadingUI.TMPro
{
    /// <inheritdoc />
    public abstract class BaseLoadingUI : BaseLoadingUI<TextMeshProUGUI>
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
#endif