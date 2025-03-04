using DG.Tweening;
using Synthesis.Input;
using Synthesis.ServiceLocators;
using Synthesis.UI.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Synthesis
{
    public class PauseView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameInputReader inputReader;
        [SerializeField] private CanvasGroup pauseGroup;
        [SerializeField] private SelectableButton resumeButton;
        [SerializeField] private SelectableButton exitButton;
        private List<SelectableButton> actionButtons;
        [SerializeField] private GameObject lastSelectedGameObject;

        [Header("Fields")]
        [SerializeField] private bool paused;

        [Header("Tweening Variables")]
        [SerializeField] private float fadeDuration = 0.5f;
        private Tween fadeTween;

        public bool Paused { get => paused; }

        private void Awake()
        {
            // Set onClick listeners
            resumeButton.Initialize(Resume);
            exitButton.Initialize(Exit);

            // Store them in a list
            actionButtons = new List<SelectableButton>() { resumeButton, exitButton };

            // Fade out immediately when the game starts
            Fade(0f, 0f, () =>
            {
                pauseGroup.interactable = false;
                pauseGroup.blocksRaycasts = false;
            });

            // Set to not paused
            paused = false;

            // Register this as a service
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void OnEnable()
        {
            inputReader.Pause += Pause;
            inputReader.Navigate += NavigateUI;
        }

        private void OnDisable()
        {
            inputReader.Pause -= Pause;
            inputReader.Navigate -= NavigateUI;
        }

        private void OnDestroy()
        {
            // Kill the fade tween if it exists
            fadeTween?.Kill();
        }

        /// <summary>
        /// Handle Navigation input for the UI
        /// </summary>
        private void NavigateUI(Vector2 direction)
        {
            // Exit case - if not paused
            if (!paused) return;

            // Exit case - there's nothing selected
            if (EventSystem.current.currentSelectedGameObject == null) return;

            // Exit case - no direction was input
            if (direction == Vector2.zero) return;

            // Exit case - a Selectable button is not selected
            if (!EventSystem.current.currentSelectedGameObject.TryGetComponent(out SelectableButton actionButton)) return;

            if (!actionButtons.Contains(actionButton)) return;

            // Get the y-direction of the navigation
            int yDirection = -(int)direction.y;

            // Set the current index
            int currentIndex = 0;

            // Iterate through the action buttons
            for (int i = 0; i < actionButtons.Count; i++)
            {
                // Check if the current button is the selected button
                if (actionButtons[i] == actionButton)
                {
                    // Set the current index
                    currentIndex = i;
                    break;
                }
            }

            SelectableButton buttonToSelect = null;

            // Loop until a Tab is selected
            while (buttonToSelect == null)
            {
                currentIndex += yDirection;

                // Check if the current index is out of bounds
                if (currentIndex < 0 || currentIndex >= actionButtons.Count)
                {
                    // Clamp the current index inside of the array bounds
                    currentIndex = currentIndex < 0 ? actionButtons.Count - 1 : 0;
                }

                // Check if the button to select is not interactable
                if (!actionButtons[currentIndex].Interactable)
                {
                    // Skip over it
                    currentIndex += yDirection;
                }

                // Check if the current index is out of bounds
                if (currentIndex < 0 || currentIndex >= actionButtons.Count)
                {
                    // Clamp the current index inside of the array bounds
                    currentIndex = currentIndex < 0 ? actionButtons.Count - 1 : 0;
                }

                // Check if the button is interactable
                if (actionButtons[currentIndex].Interactable)
                    // If so, select the button
                    buttonToSelect = actionButtons[currentIndex];
            }

            // Set the selected game object
            EventSystem.current.SetSelectedGameObject(actionButtons[currentIndex].gameObject);
        }

        /// <summary>
        /// Handle pausing input
        /// </summary>
        private void Pause(bool started)
        {
            // Exit case - if the button is pressed ebut not released
            if (started!) return;

            // Toggle paused
            paused = !paused;

            // Check if the game is paused
            if (paused)
                OnPause();
            else
                OnResume();
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        private void OnPause()
        {
            // Set the time scale to 0
            Time.timeScale = 0f;

            // Check if there's a currently selected game object
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                // Cache it for later
                lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(null);
            }

            // Fade in the pause menu
            Fade(1f, fadeDuration, () =>
            {
                // Set the pause group as active
                pauseGroup.interactable = true;
                pauseGroup.blocksRaycasts = true;

                // Set the resume button as the currently selected game object
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            });
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        private void OnResume()
        {
            // Set unpaused
            paused = false;

            // Set the time scale to 1
            Time.timeScale = 1f;

            // Deselect any selected game object
            EventSystem.current.SetSelectedGameObject(null);

            // Fade out
            Fade(0f, fadeDuration, () =>
            {
                // Set the pause group as inactive
                pauseGroup.interactable = false;
                pauseGroup.blocksRaycasts = false;

                // Check if there's a last selected game object
                if (lastSelectedGameObject != null)
                {
                    // Set it as the currently selected game object
                    EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
                }
            });
        }

        /// <summary>
        /// Resume the game via Button Click
        /// </summary>
        public void Resume()
        {
            // Exit case - if not paused
            if (!paused) return;

            // Resume the game
            OnResume();
        }

        /// <summary>
        /// Restart the running scene
        /// </summary>
        public void Restart()
        {
            // Exit case - if not paused
            if (!paused) return;

            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            // Reload the scene
            SceneManager.LoadScene(buildIndex);
        }

        /// <summary>
        /// Exit to the main menu
        /// </summary>
        public void Exit()
        {
            // Exit case - if not paused
            if (!paused) return;

            // Load the main menu
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Handle fading for the Pause Menu
        /// </summary>
        private void Fade(float endValue, float fadeDuration, TweenCallback onComplete = null)
        {
            // Kill the fade tween if it exists
            fadeTween?.Kill();

            // Set the fade tween
            fadeTween = pauseGroup.DOFade(endValue, fadeDuration);

            // Run independently of timescale
            fadeTween.SetUpdate(true);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeTween.onComplete += onComplete;
        }
    }
}
