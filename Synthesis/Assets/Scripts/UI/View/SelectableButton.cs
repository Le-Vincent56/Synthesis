using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Synthesis.UI.View
{
    public class SelectableButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private Image highlightImage;
        [SerializeField] private CanvasGroup canvasGroup;
        private RectTransform imageTransform;
        private Button button;

        [Header("Fields")]
        [SerializeField] private bool interactable;

        [Header("Tweening Variables")]
        [SerializeField] private Color glowColor;
        private Color initialColor;
        [SerializeField] private float highlightDuration;
        [SerializeField] private float glowDuration;
        private Tween colorTween;
        private Tween fadeTween;
        private Tween fadeGroupTween;
        private Tween expandTween;

        public bool Interactable { get => interactable; }

        private void OnDestroy()
        {
            // Kill the highlight tween if it exists
            fadeTween?.Kill();
            colorTween?.Kill();
            fadeGroupTween?.Kill();
            expandTween?.Kill();
        }

        /// <summary>
        /// Initialize the Selectable Button
        /// </summary>
        public void Initialize(Action onClick)
        {
            // Get components
            button = GetComponent<Button>();
            imageTransform = highlightImage.GetComponent<RectTransform>();

            // Set the initial color
            initialColor = highlightImage.color;
            initialColor.a = 1f;

            // Set button event listener
            button.onClick.AddListener(() => onClick());
        }

        /// <summary>
        /// Enable the Selectable Button
        /// </summary>
        public void Enable()
        {
            // Set interactable
            interactable = true;
            button.interactable = true;

            FadeGroup(1f, () =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }

        /// <summary>
        /// Disable the Selectable Button
        /// </summary>
        public void Disable()
        {
            // Set un-interactable
            interactable = false;
            button.interactable = false;

            FadeGroup(0.25f, () =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
        }

        public void OnSelect(BaseEventData eventData) => Highlight(true);

        public void OnDeselect(BaseEventData eventData) => Highlight(false);

        public void OnSubmit(BaseEventData eventData) => Glow();

        public void OnPointerEnter(PointerEventData eventData) => Highlight(true);

        public void OnPointerExit(PointerEventData eventData) => Highlight(false);

        public void OnPointerClick(PointerEventData eventData) => Glow();

        /// <summary>
        /// Handle highlighting for the selectable button
        /// </summary>
        private void Highlight(bool selected)
        {
            // Exit case - not active
            if (!interactable) return;

            // Check if selected
            if(selected)
            {
                // Activate selection tweens
                Fade(1f);
                Expand(1f);

                // Exit 
                return;
            }

            // Activate deselection tweens
            Fade(0f);
            Expand(0f);
        }

        /// <summary>
        /// Handle button highlighting
        /// </summary>
        private void Fade(float endValue)
        {
            // Kill the highlight tween if it exists
            fadeTween?.Kill();

            // Set the highlight tween
            fadeTween = highlightImage.DOFade(endValue, highlightDuration);

            fadeTween.SetUpdate(true);

            // Set the easing type
            fadeTween.SetEase(Ease.InQuad);
        }

        /// <summary>
        /// Handdle the button group fading
        /// </summary>
        private void FadeGroup(float endValue, TweenCallback onComplete = null)
        {
            // Kill the highlight tween if it exists
            fadeGroupTween?.Kill();

            // Set the highlight tween
            fadeGroupTween = canvasGroup.DOFade(endValue, highlightDuration);

            // Set the easing type
            fadeGroupTween.SetEase(Ease.InQuad);

            fadeGroupTween.SetUpdate(true);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeTween.onComplete += onComplete;
        }

        /// <summary>
        /// Handle button expanding
        /// </summary>
        private void Expand(float endValue, TweenCallback onComplete = null)
        {
            // Kill the expand tween if it exists
            expandTween?.Kill();

            // Set the expand tween
            expandTween = imageTransform.DOAnchorMax(new Vector2(endValue, imageTransform.anchorMax.y), highlightDuration);

            // Set the easing type
            expandTween.SetEase(Ease.InQuad);

            expandTween.SetUpdate(true);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            expandTween.onComplete += onComplete;
        }

        /// <summary>
        /// Glow the button
        /// </summary>
        private void Glow()
        {
            // Exit case - not active
            if (!interactable) return;

            // Change the color to the glow color
            ChangeColor(glowColor, () =>
            {
                // Change back to the initial color
                ChangeColor(initialColor);
            });
        }

        /// <summary>
        /// Handle button color changing
        /// </summary>
        private void ChangeColor(Color endValue, TweenCallback onComplete = null)
        {
            // Kill the color tween if it exists
            colorTween?.Kill();

            // Set the color tween
            colorTween = highlightImage.DOColor(endValue, glowDuration / 2f);

            // Set the easing type
            colorTween.SetEase(Ease.InQuad);

            colorTween.SetUpdate(true);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            colorTween.onComplete += onComplete;
        }
    }
}
