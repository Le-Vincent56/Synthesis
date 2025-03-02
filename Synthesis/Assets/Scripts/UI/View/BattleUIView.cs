using DG.Tweening;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.Timers;
using UnityEngine;
using UnityEngine.UI;
using Synthesis.EventBus.Events.Turns;
using UnityEngine.EventSystems;
using Synthesis.Mutations;
using System.Collections.Generic;

namespace Synthesis.UI.View
{
    public class BattleUIView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup turnHeader;
        [SerializeField] private CanvasGroup playerInformation;
        [SerializeField] private CanvasGroup enemyInformation;
        [SerializeField] private CanvasGroup synthesizeShop;
        private RectTransform playerInfoRect;
        private RectTransform synthesizeShopRect;
        private Text turnHeaderText;
        [SerializeField] private SelectableButton infectButton;
        [SerializeField] private SelectableButton synthesizeButton;

        [Header("Fields")]
        [SerializeField] private bool turnHeaderActive;
        private CountdownTimer turnHeaderFadeOutTimer;

        [Header("Tweening Variables")]
        [SerializeField] private float translateDuration;
        [SerializeField] private float translateAmount;
        [SerializeField] private float fadeHeaderDuration;
        [SerializeField] private float fadeEnemyInfoDuration;
        private Tween fadeTurnHeaderTween;
        private Tween translatePlayerInfoTween;
        private Tween translateSynthesizeShopTween;
        private Tween fadeEnemyInfoTween;

        private void Awake()
        {
            // Get components
            playerInfoRect = playerInformation.GetComponent<RectTransform>();
            synthesizeShopRect = synthesizeShop.GetComponent<RectTransform>();
            turnHeaderText = turnHeader.GetComponentInChildren<Text>();

            // Create the Turn Header fade out timer
            turnHeaderFadeOutTimer = new CountdownTimer(2f);
            turnHeaderFadeOutTimer.OnTimerStop += () =>
            {
                // Hide the turn headar
                HideTurnHeader();
            };

            // Initialize buttons
            infectButton.Initialize(() => EventBus<Infect>.Raise(new Infect()));
            synthesizeButton.Initialize(() => EventBus<ShowSynthesizeShop>.Raise(new ShowSynthesizeShop()));

            // Translate the player info off screen
            TranslatePlayerInfo(-translateAmount, 0f);
            TranslateSynthesizeShop(-translateAmount, 0f);
        }

        private void OnDestroy()
        {
            // Dispose of timers
            turnHeaderFadeOutTimer?.Dispose();

            // Kill any existing tweens
            fadeTurnHeaderTween?.Kill();
            translatePlayerInfoTween?.Kill();
            translateSynthesizeShopTween?.Kill();
            fadeEnemyInfoTween?.Kill();
        }

        /// <summary>
        /// Show the Turn Header
        /// </summary>
        public void ShowTurnHeader(string text)
        {
            // Check if the Turn Header is already active
            if (turnHeaderActive)
            {
                // Stop the fade out timer
                turnHeaderFadeOutTimer.Pause(true);

                // Fade out the current header
                FadeTurnHeader(0f, () =>
                {
                    // Show the text
                    SetAndShowHeaderText(text);
                });

                return;
            }

            // Set and show the header
            SetAndShowHeaderText(text);
        }

        /// <summary>
        /// Set and show the header text
        /// </summary>
        private void SetAndShowHeaderText(string text)
        {
            // Set the text
            turnHeaderText.text = text;

            // Fade in the Turn Header
            FadeTurnHeader(1f, () => turnHeaderFadeOutTimer.Start());

            // Set active turn header
            turnHeaderActive = true;
        }

        /// <summary>
        /// Hide the Turn Header
        /// </summary>
        public void HideTurnHeader()
        {
            // Ensure the fade out timer has stopped
            turnHeaderFadeOutTimer.Pause(true);

            // Set inactive turn header
            turnHeaderActive = false;

            // Fade out the Turn Header
            FadeTurnHeader(0f, () =>
            {
                // Reset the text
                turnHeaderText.text = string.Empty;
            });
        }

        /// <summary>
        /// Show the Player Information panel
        /// </summary>
        public void ShowPlayerInfo()
        {
            TranslatePlayerInfo(translateAmount, translateDuration, () =>
            {
                // Select the Infect button
                EventSystem.current.SetSelectedGameObject(infectButton.gameObject);
            });
        }

        /// <summary>
        /// Hide the Player Information panel
        /// </summary>
        public void HidePlayerInfo() => TranslatePlayerInfo(-translateAmount, translateDuration);

        /// <summary>
        /// Show the Synthesize Shop panel
        /// </summary>
        public void ShowSynthesizeShop(List<MutationCard> selectedMutations)
        {
            // Hide the Player Info
            TranslatePlayerInfo(-translateAmount, translateDuration, () =>
            {
                // Show the Synthesize Shop
                TranslateSynthesizeShop(translateAmount, translateDuration, () =>
                {
                    EventSystem.current.SetSelectedGameObject(selectedMutations[0].gameObject);
                });
            });
        }

        /// <summary>
        /// Hide the Syntehsize Shop panel
        /// </summary>
        public void HideSynthesizeShop() => TranslateSynthesizeShop(-translateAmount, translateDuration);

        /// <summary>
        /// Handle translating the Player Information
        /// </summary>
        private void TranslatePlayerInfo(float translateAmount, float duration, TweenCallback onComplete = null)
        {
            // Kill the translate tween if it exists
            translatePlayerInfoTween?.Kill();

            // Set the translate tween
            translatePlayerInfoTween = playerInfoRect.DOAnchorPosY(
                playerInfoRect.anchoredPosition.y + translateAmount,
                duration
            );

            translatePlayerInfoTween.SetEase(Ease.InQuad);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            translatePlayerInfoTween.onComplete += onComplete;
        }

        /// <summary>
        /// Handle translating the Synthesize Shop
        /// </summary>
        private void TranslateSynthesizeShop(float translateAmount, float duration, TweenCallback onComplete = null)
        {
            // Kill the the translate tween if it exists
            translateSynthesizeShopTween?.Kill();

            // Set the translate tween
            translateSynthesizeShopTween = synthesizeShopRect.DOAnchorPosY(
                synthesizeShopRect.anchoredPosition.y + translateAmount,
                duration
            );

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            translateSynthesizeShopTween.onComplete += onComplete;
        }

        /// <summary>
        /// Handle fading the Turn Header
        /// </summary>
        private void FadeTurnHeader(float endValue, TweenCallback onComplete = null)
        {
            // Kill the fade tween if it exists
            fadeTurnHeaderTween?.Kill();

            // Set the fade tween
            fadeTurnHeaderTween = turnHeader.DOFade(endValue, fadeHeaderDuration);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeTurnHeaderTween.onComplete += onComplete;
        }

        /// <summary>
        /// Hanldes fading the Enemy Information
        /// </summary>
        private void FadeEnemyInfo(float endValue, TweenCallback onComplete = null)
        {
            // Kill the fade tween if it exists
            fadeEnemyInfoTween?.Kill();

            // Set the fade tween
            fadeEnemyInfoTween = turnHeader.DOFade(endValue, fadeEnemyInfoDuration);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeEnemyInfoTween.onComplete += onComplete;
        }
    }
}
