using BUCK.LoadingUI;
using UnityEngine;

namespace BUCK.Examples
{
    public class LoadingSpinnerExample : MonoBehaviour
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
                if (_timer > 5)
                {
                    _stopped = true;
                    Loading.Stop("Loaded!", 1);
                }
                else
                {
                    _timer += Time.smoothDeltaTime;
                }
            }
        }
    }
}