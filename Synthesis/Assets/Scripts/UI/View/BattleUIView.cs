using DG.Tweening;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.Timers;
using UnityEngine;
using UnityEngine.UI;
using Synthesis.EventBus.Events.Turns;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Synthesis.Input;
using Synthesis.ServiceLocators;
using Synthesis.EventBus.Events.Battle;

namespace Synthesis.UI.View
{
    public class BattleUIView : MonoBehaviour
    {
        [Header("References - General")]
        [SerializeField] private GameInputReader inputReader;
        [SerializeField] private CanvasGroup turnHeader;
        [SerializeField] private CanvasGroup playerInformation;
        [SerializeField] private CanvasGroup enemyInformation;
        [SerializeField] private CanvasGroup synthesizeShop;
        private PauseView pauseView;
        private RectTransform playerInfoRect;
        private RectTransform synthesizeShopRect;
        private Text turnHeaderText;

        [Header("References - Actions")]
        [SerializeField] private SelectableButton infectButton;
        [SerializeField] private SelectableButton synthesizeButton;
        private List<SelectableButton> actionButtons;
        private List<MutationCard> currentMutationCards;

        [Header("References - Turns")]
        [SerializeField] private Text currentTurn;
        [SerializeField] private Text totalTurns;
        private RectTransform turnsRemainingRect;
        private RectTransform totalTurnsRect;

        [Header("References - Combat Rating")]
        [SerializeField] private Text combatRatingDisplay;
        [SerializeField] private Text currentCombatRating;
        [SerializeField] private Text targetCombatRating;
        private RectTransform combatRatingDisplayRect;
        private RectTransform currentCombatRatingRect;
        private RectTransform targetCombatRatingRect;

        [Header("References - Wilt")]
        [SerializeField] private Text currentWilt;
        [SerializeField] private Text totalWilt;
        [SerializeField] private Image wiltFill;
        [SerializeField] private Text currentWiltText;
        private RectTransform wiltFillRect;
        private RectTransform currentWiltRect;

        [Header("Fields")]
        [SerializeField] private bool actionsShown;
        [SerializeField] private bool synthesizeShopShown;
        [SerializeField] private bool turnHeaderActive;
        private Color turnsInitialColor;
        [SerializeField] private Color textHighlightColor;
        private Vector3 turnsInitialScale;
        private Vector3 turnsMaxScale;
        private CountdownTimer turnHeaderFadeOutTimer;

        [Header("Tweening Variables")]
        [SerializeField] private float turnsScaleAmount;
        [SerializeField] private float turnsScaleDuration;
        [SerializeField] private float translateDuration;
        [SerializeField] private float translateAmount;
        [SerializeField] private float fadeHeaderDuration;
        [SerializeField] private float wiltFillDuration;
        [SerializeField] private float combatRatingDisplayDuration;
        [SerializeField] private float fadeEnemyInfoDuration;
        private Tween fadeTurnHeaderTween;
        private Tween scaleTurnsTween;
        private Tween colorTurnsTween;
        private Tween scaleTurnsTotalTween;
        private Tween colorTurnsTotalTween;
        private Tween scaleCurrentRatingTween;
        private Tween colorCurrentRatingTween;
        private Tween scaleTotalRatingTween;
        private Tween colorTotalRatingTween;
        private Tween translatePlayerInfoTween;
        private Tween translateSynthesizeShopTween;
        private Tween wiltFillTween;
        private Tween wiltTextTween;
        private Tween wiltNumberTween;
        private Tween fadeEnemyInfoTween;
        private Tween combatRatingDisplayFadeTween;
        private Tween combatRatingDisplayScaleTween;
        private Tween combatRatingNumberTween;

        public List<MutationCard> CurrentMutationCards { get => currentMutationCards; }

        private void Awake()
        {
            // Get components
            playerInfoRect = playerInformation.GetComponent<RectTransform>();
            synthesizeShopRect = synthesizeShop.GetComponent<RectTransform>();
            turnsRemainingRect = currentTurn.GetComponent<RectTransform>();
            totalTurnsRect = totalTurns.GetComponent<RectTransform>();
            combatRatingDisplayRect = combatRatingDisplay.GetComponent<RectTransform>();
            currentCombatRatingRect = currentCombatRating.GetComponent<RectTransform>();
            targetCombatRatingRect = targetCombatRating.GetComponent<RectTransform>();
            wiltFillRect = wiltFill.GetComponent<RectTransform>();
            currentWiltRect = currentWilt.GetComponent<RectTransform>();
            turnHeaderText = turnHeader.GetComponentInChildren<Text>();

            // Set tween variables
            turnsInitialColor = currentTurn.color;
            turnsInitialScale = turnsRemainingRect.localScale;
            turnsMaxScale = turnsInitialScale * turnsScaleAmount;

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

            // Create the Action buttons array
            actionButtons = new List<SelectableButton>()
            {
                infectButton,
                synthesizeButton
            };

            // Translate the player info off screen
            TranslatePlayerInfo(-translateAmount, 0f);
            TranslateSynthesizeShop(-translateAmount, 0f);
        }

        private void OnEnable()
        {
            inputReader.Navigate += NavigateUI;
        }

        private void OnDisable()
        {
            inputReader.Navigate -= NavigateUI;
        }

        private void Start()
        {
            // Get services
            pauseView = ServiceLocator.ForSceneOf(this).Get<PauseView>();
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
            scaleCurrentRatingTween?.Kill();
            colorCurrentRatingTween?.Kill();
            scaleTotalRatingTween?.Kill();
            colorTotalRatingTween?.Kill();
            wiltFillTween?.Kill();
            wiltTextTween?.Kill();
            wiltNumberTween?.Kill();
            scaleTurnsTween?.Kill();
            colorTurnsTween?.Kill();
            scaleTurnsTotalTween?.Kill();
            colorTurnsTotalTween?.Kill();
            combatRatingDisplayScaleTween?.Kill();
            combatRatingDisplayFadeTween?.Kill();
            combatRatingNumberTween?.Kill();
        }

        /// <summary>
        /// Handle Navigation input for the UI
        /// </summary>
        private void NavigateUI(Vector2 direction)
        {
            // Exit case - if paused
            if (pauseView.Paused) return;

            // Exit case - there's nothing selected
            if(EventSystem.current.currentSelectedGameObject == null) return;

            // Exit case - no direction was input
            if (direction == Vector2.zero) return;

            // Check if the actions are being shown
            if (actionsShown)
            {
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

                return;
            }

            // Check if the Synthesize Shop is being shown
            if(synthesizeShopShown)
            {
                // Exit case - a Selectable button is not selected
                if (!EventSystem.current.currentSelectedGameObject.TryGetComponent(out MutationCard card)) return;

                // Get the y-direction of the navigation
                int xDirection = (int)direction.x;

                // Set the current index
                int currentIndex = 0;

                // Iterate through the current mutation cards
                for (int i = 0; i < currentMutationCards.Count; i++)
                {
                    // Check if the current card is the selected card
                    if (currentMutationCards[i] == card)
                    {
                        // Set the current index
                        currentIndex = i;
                        break;
                    }
                }

                currentIndex += xDirection;

                // Check if the current index is out of bounds
                if (currentIndex < 0 || currentIndex >= currentMutationCards.Count)
                {
                    // Clamp the current index inside of the array bounds
                    currentIndex = currentIndex < 0 ? currentMutationCards.Count - 1 : 0;
                }

                // Set the selected game object
                EventSystem.current.SetSelectedGameObject(currentMutationCards[currentIndex].gameObject);

                return;
            }
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
        /// Update the turns remaining in combat
        /// </summary>
        public void UpdateTurns(int currentTurn, int totalTurns)
        {
            // Get the last turn values
            int lastCurrentTurn = int.Parse(this.currentTurn.text);
            int lastTotalTurns = int.Parse(this.totalTurns.text);

            // Set the text
            this.currentTurn.text = currentTurn.ToString();
            this.totalTurns.text = totalTurns.ToString();

            // Check if there was a change in the current turn
            if(lastCurrentTurn != currentTurn)
            {
                // Scale the text
                Scale(scaleTurnsTween, turnsRemainingRect, turnsMaxScale, turnsScaleDuration / 2f, () =>
                {
                    Scale(scaleTurnsTween, turnsRemainingRect, turnsInitialScale, turnsScaleDuration / 2f);
                });

                // Color the text
                Color(colorTurnsTween, this.currentTurn, textHighlightColor, turnsScaleDuration / 2f, () =>
                {
                    Color(colorTurnsTween, this.currentTurn, turnsInitialColor, turnsScaleDuration / 2f);
                });
            }

            // Check if there was a change in the total turns
            if (lastTotalTurns != totalTurns)
            {
                // Scale the text
                Scale(scaleTurnsTotalTween, totalTurnsRect, turnsMaxScale, turnsScaleDuration / 2f, () =>
                {
                    Scale(scaleTurnsTotalTween, totalTurnsRect, turnsInitialScale, turnsScaleDuration / 2f);
                });

                // Color the text
                Color(colorTurnsTotalTween, this.totalTurns, textHighlightColor, turnsScaleDuration / 2f, () =>
                {
                    Color(colorTurnsTotalTween, this.totalTurns, turnsInitialColor, turnsScaleDuration / 2f);
                });
            }
        }

        /// <summary>
        /// Update the Target Combat Rating text
        /// </summary>
        public void UpdateTargetCombatRating(int targetCombatRating)
        {
            // Update the text
            this.targetCombatRating.text = targetCombatRating.ToString();

            // Scale the text
            Scale(scaleTotalRatingTween, targetCombatRatingRect, turnsMaxScale, turnsScaleDuration / 2f, () =>
            {
                Scale(scaleTotalRatingTween, targetCombatRatingRect, turnsInitialScale, turnsScaleDuration / 2f);
            });

            // Color the text
            Color(colorTotalRatingTween, this.targetCombatRating, textHighlightColor, turnsScaleDuration / 2f, () =>
            {
                Color(colorTotalRatingTween, this.targetCombatRating, turnsInitialColor, turnsScaleDuration / 2f);
            });
        }

        /// <summary>
        /// Update the Current Combat Rating text
        /// </summary>
        public void UpdateCurrentCombatRating(int currentCombatRating)
        {
            // Update the text
            this.currentCombatRating.text = currentCombatRating.ToString();

            // Scale the text
            Scale(scaleCurrentRatingTween, currentCombatRatingRect, turnsMaxScale, turnsScaleDuration / 2f, () =>
            {
                Scale(scaleCurrentRatingTween, currentCombatRatingRect, turnsInitialScale, turnsScaleDuration / 2f);
            });

            // Color the text
            Color(colorCurrentRatingTween, this.currentCombatRating, textHighlightColor, turnsScaleDuration / 2f, () =>
            {
                Color(colorCurrentRatingTween, this.currentCombatRating, turnsInitialColor, turnsScaleDuration / 2f);
            });
        }

        /// <summary>
        /// Update the combat rating display
        /// </summary>
        public void UpdateCombatRatingDisplay(int currentCombatRating)
        {
            int previousCombatRating = int.Parse(this.currentCombatRating.text);

            // Update the number climbing

            FadeText(combatRatingDisplayFadeTween, combatRatingDisplay, 1f, 0.15f, () =>
            {
                // Scale upwards
                Scale(combatRatingDisplayScaleTween, combatRatingDisplayRect, turnsMaxScale, 0.25f, () =>
                {
                    UpdateInCombatRatingNumber(previousCombatRating, currentCombatRating, () =>
                    {
                        // Scale downwards
                        Scale(combatRatingDisplayScaleTween, combatRatingDisplayRect, turnsInitialScale, 0.25f, () =>
                        {
                            // Fade out
                            FadeText(combatRatingDisplayFadeTween, combatRatingDisplay, 0f, 0.15f, () =>
                            {
                                EventBus<CombatRatingFinalized>.Raise(new CombatRatingFinalized() { CombatRating = currentCombatRating });

                            });
                        });
                    });
                });
            });
        }

        /// <summary>
        /// Update the current Wilt text
        /// </summary>
        public void UpdateCurrentWilt(int currentWilt, int totalWilt)
        {
            // Get the previous wilt value
            int previousWilt = int.Parse(this.currentWilt.text);

            // Get the fill percentage
            float fillPercentage = (float)currentWilt / totalWilt;

            // Lerp the fill
            FillWilt(fillPercentage, previousWilt, currentWilt);
        }

        /// <summary>
        /// Update the total Wilt text
        /// </summary>
        public void UpdateTotalWilt(int totalWilt)
        {
            // Update the text
            this.totalWilt.text = totalWilt.ToString();
        }

        /// <summary>
        /// Set whether or not the player can synthesize
        /// </summary>
        public void SetCanSynthesize(bool canSynthesize)
        {
            // Check if the player can synthesize
            if (canSynthesize)
            {
                // Enable the synthesize button
                synthesizeButton.Enable();
            }
            else
            {
                // Disable the synthesize button
                synthesizeButton.Disable();
            }
        }

        /// <summary>
        /// Show the Player Information panel
        /// </summary>
        public void ShowPlayerInfo()
        {
            TranslatePlayerInfo(translateAmount, translateDuration, () =>
            {
                // Set that actions are shown
                actionsShown = true;

                // Select the Infect button
                EventSystem.current.SetSelectedGameObject(infectButton.gameObject);
            });
        }

        /// <summary>
        /// Hide the Player Information panel
        /// </summary>
        public void HidePlayerInfo() => TranslatePlayerInfo(-translateAmount, translateDuration, () => actionsShown = false);

        /// <summary>
        /// Show the Synthesize Shop panel
        /// </summary>
        public void ShowSynthesizeShop(List<MutationCard> selectedMutations)
        {
            // Set the selected mutations
            currentMutationCards = selectedMutations;

            // Hide the Player Info
            TranslatePlayerInfo(-translateAmount, translateDuration, () =>
            {
                // Set that actions are not shown
                actionsShown = false;

                // Show the Synthesize Shop
                TranslateSynthesizeShop(translateAmount, translateDuration, () =>
                {
                    // Set that the synthesize shop is shown
                    synthesizeShopShown = true;

                    EventSystem.current.SetSelectedGameObject(selectedMutations[0].gameObject);
                });
            });
        }

        /// <summary>
        /// Hide the Syntehsize Shop panel
        /// </summary>
        public void HideSynthesizeShop(MutationCardPool pool)
        {
            TranslateSynthesizeShop(-translateAmount, translateDuration, () =>
            {
                // Iterate through each Mutation Card
                foreach (MutationCard card in currentMutationCards)
                {
                    // Release each card back to the pool
                    pool.Release(card);
                }

                // Set that the synthesize shop is not shown
                synthesizeShopShown = false;

                // Clear the list of current mutation cards
                currentMutationCards.Clear();
            });
        } 

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
        /// Handle fading for text
        /// </summary>
        private void FadeText(Tween fadeTween, Text text, float endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill the fade tween if it exists
            fadeTween?.Kill();

            // Set the fade tween
            fadeTween = text.DOFade(endValue, duration);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeTween.onComplete += onComplete;
        }

        /// <summary>
        /// Handle scaling tweens
        /// </summary>
        private void Scale(Tween scaleTween, RectTransform rectTransform, Vector3 endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill the translate tween if it exists
            scaleTween?.Kill();

            // Set the translate tween
            scaleTween = rectTransform.DOScale(
                endValue,
                duration
            );

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            scaleTween.onComplete += onComplete;
        }

        /// <summary>
        /// Handle coloring tweens
        private void Color(Tween colorTween, Text text, Color endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill the translate tween if it exists
            colorTween?.Kill();

            // Set the translate tween
            colorTween = text.DOColor(
                endValue,
                duration
            );

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            colorTween.onComplete += onComplete;
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
        /// Handle filling the Wilt bar
        /// </summary>
        private void FillWilt(float endValue, int previousWilt, int currentWilt, TweenCallback onComplete = null)
        {
            // Kill the fill tween if it exists
            wiltFillTween?.Kill();
            wiltTextTween?.Kill();

            // Set the fill tween
            wiltFillTween = wiltFill.DOFillAmount(endValue, wiltFillDuration);

            // Calculate target X position
            float targetX = wiltFillRect.rect.width * endValue;

            // Animate Current Wilt Text to the right edge of the Fill
            wiltTextTween = currentWiltRect.DOAnchorPosX(targetX, wiltFillDuration);

            // Animate the number change
            wiltNumberTween = DOVirtual.Int(previousWilt, currentWilt, wiltFillDuration, value =>
            {
                this.currentWilt.text = value.ToString();
            });

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            wiltFillTween.onComplete += onComplete;
        }

        /// <summary>
        /// Update the number climbing for the in-combat combat rating display
        /// </summary>
        public void UpdateInCombatRatingNumber(int previousRating, int currentRating, TweenCallback onComplete = null)
        {
            // Kill the combat rating tween if it exists
            combatRatingNumberTween?.Kill();

            combatRatingNumberTween = DOVirtual.Int(previousRating, currentRating, combatRatingDisplayDuration, value =>
            {
                combatRatingDisplay.text = value.ToString();
            });

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            combatRatingNumberTween.onComplete += onComplete;
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
