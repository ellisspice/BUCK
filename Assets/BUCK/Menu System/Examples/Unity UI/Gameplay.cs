using UnityEngine;

namespace BUCK.Examples.UnityUI
{
    public class Gameplay : MonoBehaviour
    {
        public float GameTimer { get; private set; } = 10f;
        public static bool PlayingGame = true;
        public static bool GameOngoing = true;
        
        private void Start()
        {
            GameTimer = 10;
        }
        
        private void Update()
        {
            if (GameOngoing && PlayingGame)
            {
                GameTimer -= Time.smoothDeltaTime;
            }
        }
    }
}