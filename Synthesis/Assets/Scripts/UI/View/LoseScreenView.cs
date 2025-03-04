using DG.Tweening;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.UI.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Synthesis
{
    public class LoseScreenView : MonoBehaviour
    {
        [Header("References")]
        private CanvasGroup canvasGroup;
        [SerializeField] private SelectableButton mainMenuButton;

        [Header("Tweening Variables")]
        [SerializeField] private float fadeDuration = 0.5f;
        private Tween fadeTween;

        private EventBinding<LoseBattle> onLoseBattle;

        private void Awake()
        {
            // Get components
            canvasGroup = GetComponent<CanvasGroup>();

            // Initialize the Selectable Button
            mainMenuButton.Initialize(() => SceneManager.LoadScene(0));
        }

        private void OnEnable()
        {
            onLoseBattle = new EventBinding<LoseBattle>(ShowLoseScreen);
            EventBus<LoseBattle>.Register(onLoseBattle);
        }

        private void OnDisable()
        {
            EventBus<LoseBattle>.Deregister(onLoseBattle);
        }

        private void OnDestroy()
        {
            // Kill the fade tween if it exists
            fadeTween?.Kill();
        }

        /// <summary>
        /// Show the Lose Screen
        /// </summary>
        private void ShowLoseScreen()
        {
            // Set the selected game object to null
            EventSystem.current.SetSelectedGameObject(null);

            // Fade in the lose screen
            Fade(1f, () =>
            {
                // Allow the canvas group to be interacted with
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;

                // Set the selected game object to the main menu button
                EventSystem.current.SetSelectedGameObject(mainMenuButton.gameObject);
            });
        }

        /// <summary>
        /// Handle fading in the Lose Screen
        /// </summary>
        private void Fade(float endValue, TweenCallback onComplete = null)
        {
            // Kill the fade tween if it exists
            fadeTween?.Kill();

            // Set the fade tween
            fadeTween = canvasGroup.DOFade(endValue, fadeDuration);

            // Independently update the fade tween
            fadeTween.SetUpdate(true);

            // Exit case - there is no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeTween.onComplete += onComplete;
        }
    }
}
