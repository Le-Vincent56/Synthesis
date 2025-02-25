using Synthesis.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Synthesis
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameInputReader inputReader;

        [SerializeField] private GameObject pauseMenuCanvas;
        private bool paused;

        private void OnEnable()
        {
            inputReader.Cancel += HandlePause;
        }

        private void OnDisable()
        {
            inputReader.Cancel -= HandlePause;
        }

        private void HandlePause(bool started)
        {
            // Exit case - if the button is released
            if (!started) return;

            // Check if paused
            if(paused)
            {
                // Resume the game
                ResumeGame();
                return;
            }

            // Pause the game
            PauseGame();
        }

        public void PauseGame()
        {
            // Pause the game
            Time.timeScale = 0;
            pauseMenuCanvas.SetActive(true);
            paused = true;

        }

        public void ResumeGame()
        {
            // Resume the game
            Time.timeScale = 1;
            pauseMenuCanvas.SetActive(false);
            paused = false;
        }

        public void ToMainMenu()
        {
            // Load the main menu scene
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
