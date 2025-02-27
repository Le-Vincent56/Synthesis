using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Synthesis
{
    public class SelectableButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerDownHandler
    {
        [Header("References")]
        [SerializeField] private Image image;
        private RectTransform imageTransform;
        private Button button;

        [Header("Tweening Variables")]
        [SerializeField] private Color glowColor;
        private Color initialColor;
        [SerializeField] private float highlightDuration;
        [SerializeField] private float glowDuration;
        private Tween colorTween;
        private Tween fadeTween;
        private Tween expandTween;

        private void Awake()
        {
            // Get components
            button = GetComponent<Button>();
            imageTransform = image.GetComponent<RectTransform>();

            // Set the initial color
            initialColor = image.color;
            initialColor.a = 1f;
        }

        private void OnDestroy()
        {
            // Kill the highlight tween if it exists
            fadeTween?.Kill();
            expandTween?.Kill();
        }

        public void OnSelect(BaseEventData eventData) => Highlight(true);

        public void OnDeselect(BaseEventData eventData) => Highlight(false);

        public void OnSubmit(BaseEventData eventData) => Glow();

        public void OnPointerDown(PointerEventData eventData) => Glow();

        /// <summary>
        /// Handle highlighting for the selectable button
        /// </summary>
        private void Highlight(bool selected)
        {
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
            fadeTween = image.DOFade(endValue, highlightDuration);

            // Set the easing type
            fadeTween.SetEase(Ease.InQuad);
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
            colorTween = image.DOColor(endValue, glowDuration / 2f);

            // Set the easing type
            colorTween.SetEase(Ease.InQuad);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            colorTween.onComplete += onComplete;
        }
    }
}
