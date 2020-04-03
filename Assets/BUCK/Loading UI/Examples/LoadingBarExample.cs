using BUCK.LoadingUI;
using UnityEngine;

namespace BUCK.Examples
{
    public class LoadingBarExample : MonoBehaviour
    {
        private bool _stopped;

        private float _timer;
        private void Start()
        {
            Loading.Start("Loading!");
        }

        private void Update()
        {
            if (!_stopped)
            {
                if (_timer >= 1)
                {
                    _stopped = true;
                    Loading.Stop("Loaded!", 1);
                }
                else
                {
                    _timer += Random.Range(0f, 0.5f) * Time.smoothDeltaTime;
                    Loading.SetValue(_timer);
                }
            }
        }
    }
}