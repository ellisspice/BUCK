#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BUCK.VideoUI.UnityUI
{
    /// <inheritdoc />
    [AddComponentMenu("Video/Video Player UI")]
    public class VideoPlayerUI : BaseVideoPlayerUI<Text, Dropdown>
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