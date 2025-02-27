using DG.Tweening;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.EventBus.Events.UI;
using Synthesis.Timers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Synthesis.UI.View
{
    public class BattleUIView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup turnHeader;
        [SerializeField] private CanvasGroup playerInformation;
        [SerializeField] private CanvasGroup enemyInformation;
        private RectTransform playerInfoRect;
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
        private Tween fadeEnemyInfoTween;

        private EventBinding<ShowTurnHeader> onShowTurnHeader;
        private EventBinding<HideTurnHeader> onHideTurnHeader;
        private EventBinding<ShowPlayerInfo> onShowPlayerInfo;
        private EventBinding<HidePlayerInfo> onHidePlayerInfo;

        private void Awake()
        {
            // Get components
            playerInfoRect = playerInformation.GetComponent<RectTransform>();
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
            synthesizeButton.Initialize(() => EventBus<Synthesize>.Raise(new Synthesize()));

            // Translate the player info off screen
            TranslatePlayerInfo(-translateAmount, 0f);
        }

        private void OnEnable()
        {
            onShowTurnHeader = new EventBinding<ShowTurnHeader>(ShowTurnHeader);
            EventBus<ShowTurnHeader>.Register(onShowTurnHeader);

            onHideTurnHeader = new EventBinding<HideTurnHeader>(HideTurnHeader);
            EventBus<HideTurnHeader>.Register(onHideTurnHeader);

            onShowPlayerInfo = new EventBinding<ShowPlayerInfo>(ShowPlayerInfo);
            EventBus<ShowPlayerInfo>.Register(onShowPlayerInfo);

            onHidePlayerInfo = new EventBinding<HidePlayerInfo>(HidePlayerInfo);
            EventBus<HidePlayerInfo>.Register(onHidePlayerInfo);
        }

        private void OnDisable()
        {
            EventBus<ShowTurnHeader>.Deregister(onShowTurnHeader);
            EventBus<HideTurnHeader>.Deregister(onHideTurnHeader);
            EventBus<ShowPlayerInfo>.Deregister(onShowPlayerInfo);
            EventBus<HidePlayerInfo>.Deregister(onHidePlayerInfo);
        }

        private void OnDestroy()
        {
            // Dispose of timers
            turnHeaderFadeOutTimer?.Dispose();

            // Kill any existing tweens
            fadeTurnHeaderTween?.Kill();
            translatePlayerInfoTween?.Kill();
            fadeEnemyInfoTween?.Kill();
        }

        /// <summary>
        /// Show the Turn Header
        /// </summary>
        private void ShowTurnHeader(ShowTurnHeader eventData)
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
                    SetAndShowHeaderText(eventData.Text);
                });

                return;
            }

            // Set and show the header
            SetAndShowHeaderText(eventData.Text);
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
        private void HideTurnHeader()
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
        private void ShowPlayerInfo()
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
        private void HidePlayerInfo() => TranslatePlayerInfo(-translateAmount, translateDuration);

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
