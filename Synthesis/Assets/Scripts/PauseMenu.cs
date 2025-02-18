using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Synthesis
{
    public class PauseMenu : MonoBehaviour
    {

        [SerializeField] private GameObject pauseMenuCanvas;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            // Pause the game
            Time.timeScale = 0;
            pauseMenuCanvas.SetActive(true);

        }

        public void ResumeGame()
        {
            // Resume the game
            Time.timeScale = 1;
            pauseMenuCanvas.SetActive(false);
        }

        public void ToMainMenu()
        {
            // Load the main menu scene
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
