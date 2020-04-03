#if TEXT_MESH_PRO_INCLUDED
using System;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BUCK.VideoUI.TMPro
{
    /// <inheritdoc />
    [AddComponentMenu("Video/Video Player UI (Text Mesh Pro)")]
    public class VideoPlayerUI : BaseVideoPlayerUI<TextMeshProUGUI, TMP_Dropdown>
    {
        /// <inheritdoc />
        protected override void UpdateUI(bool playing)
        {
            base.UpdateUI(playing);
            if (_playbackDropdown)
            {
                _playbackDropdown.ClearOptions();
                var options = _playbackSpeedOptions;
                if (!options.Contains(_player.playbackSpeed))
                {
                    options = options.Concat(new[] {_player.playbackSpeed}).OrderBy(s => s).ToArray();
                }
                var optionString = options.Select(o => o.ToString(CultureInfo.CurrentCulture)).ToList();
                _playbackDropdown.AddOptions(optionString);
                _playbackDropdown.value = _playbackDropdown.options.FindIndex(o => o.text == _player.playbackSpeed.ToString(CultureInfo.CurrentCulture));
            }
        }

        /// <inheritdoc />
        public override void UpdatePlaybackSpeed()
        {
            _player.playbackSpeed = float.Parse(_playbackDropdown.options[_playbackDropdown.value].text);
        }

        /// <inheritdoc />
        protected override void UpdateTimerText(TimeSpan length)
        {
            var time = TimeSpan.FromSeconds(_player.time);
            var text = (time.Hours > 0 ? time.Hours + ":" + time.Minutes.ToString("00") + ":" : time.Minutes + ":") + time.Seconds.ToString("00");
            text += " / " + (length.Hours > 0 ? length.Hours + ":" + length.Minutes.ToString("00") + ":" : length.Minutes + ":") + length.Seconds.ToString("00");
            _timer.text = text;
        }
    }
}
#endif