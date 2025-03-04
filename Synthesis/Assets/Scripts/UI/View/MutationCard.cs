using DG.Tweening;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Mutations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Synthesis.UI.View
{
    public class MutationCard : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private MutationStrategy mutation;
        private RectTransform rectTransform;
        private Text nameText;
        private Text descriptionText;

        [Header("Tweening Variables")]
        [SerializeField] private float scaleDuration;
        [SerializeField] private float scaleAmount;
        [SerializeField] private float submitDuration;
        private Tween scaleTween;

        private Vector3 initialScale;
        private Vector3 maxScale;

        public Action OnClick = delegate { };

        public void OnDestroy()
        {
            // Kill any tweens if they exist
            scaleTween?.Kill();
        }

        /// <summary>
        /// Initialize the Mutation Card
        /// </summary>
        public void Initialize()
        {
            // Get the child Texts
            List<Text> texts = new List<Text>();
            GetComponentsInChildren(texts);

            // Set the Texts
            nameText = texts[0];
            descriptionText = texts[1];

            initialScale = transform.localScale;
            maxScale = initialScale * scaleAmount;

            // Set Rect Transform
            rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Set the Mutation Card data
        /// </summary>
        public void SetData(MutationStrategy mutation)
        {
            this.mutation = mutation;
            nameText.text = mutation.Name;
            descriptionText.text = mutation.Description;
        }

        /// <summary>
        /// Reset the Mutation Card data
        /// </summary>
        public void ResetData()
        {
            mutation = null;
            nameText.text = string.Empty;
            descriptionText.text = string.Empty;
        }

        /// <summary>
        /// Select the Mutation card
        /// </summary>
        private void Select()
        {
            // Scale and translate
            Scale(maxScale, scaleDuration);
        }

        /// <summary>
        /// Deselect the Mutation Card
        /// </summary>
        private void Deselect()
        {
            Scale(initialScale, scaleDuration);
        }

        /// <summary>
        /// Submit the Mutation Card
        /// </summary>
        private void Submit()
        {
            // Scale inwards
            Scale(initialScale, submitDuration / 2f, () =>
            {
                // Scale back outwards and invoke the OnClick action
                Scale(maxScale, submitDuration / 2f, () =>
                {
                    // Raise the Synthesize event
                    EventBus<Synthesize>.Raise(new Synthesize()
                    {
                        Mutation = mutation
                    });

                    // Remove any objects from the Event System
                    EventSystem.current.SetSelectedGameObject(null);
                });
            });
        }

        /// <summary>
        /// Handle scale tweening for the Mutation Card
        /// </summary>
        private void Scale(Vector3 endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill the scale tween if it exists
            scaleTween?.Kill();

            // Set the scale tween
            scaleTween = rectTransform.DOScale(endValue, duration);

            // Exit case - there's no completion action
            if (onComplete == null) return;
            
            // Set the completion action
            scaleTween.onComplete += onComplete;
        }

        public void OnSelect(BaseEventData eventData) => Select();

        public void OnDeselect(BaseEventData eventData) => Deselect();

        public void OnSubmit(BaseEventData eventData) => Submit();
        public void OnPointerEnter(PointerEventData eventData) => Select();
        public void OnPointerExit(PointerEventData eventData) => Deselect();

        public void OnPointerClick(PointerEventData eventData) => Submit();
    }
}
