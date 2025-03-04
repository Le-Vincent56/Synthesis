using Synthesis.Input;
using UnityEngine;

namespace Synthesis
{
    public class GameSpeedUp : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameInputReader inputReader;

        [Header("Fields")]
        [SerializeField] private float speedUpMult;

        private void OnEnable()
        {
            inputReader.SpeedUp += SpeedUp;
        }

        private void OnDisable()
        {
            inputReader.SpeedUp -= SpeedUp;
        }

        /// <summary>
        /// Speed up the time of the game
        /// </summary>
        private void SpeedUp(bool started)
        {
            // Check if the button is down
            if (started)
            {
                // Set the time scale to the speed up multiplier
                Time.timeScale = speedUpMult;

                return;
            }

            // Set the time scale to normal
            Time.timeScale = 1f;
        }
    }
}
