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
    public class MutationTag : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private MutationStrategy mutation;
        private RectTransform rectTransform;
        private Text nameText;

        [Header("Tweening Variables")]
        [SerializeField] private float scaleDuration;
        [SerializeField] private float scaleAmount;
        [SerializeField] private float submitDuration;
        [SerializeField] private bool interactable = true;
        private Tween scaleTween;

        private Vector3 initialScale;
        private Vector3 maxScale;

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

            initialScale = transform.localScale;
            maxScale = initialScale * scaleAmount;

            // Set Rect Transform
            rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Set the Mutation Card data
        /// </summary>
        public void SetData(MutationStrategy mutation, bool interactable = true)
        {
            this.interactable = interactable;
            this.mutation = mutation;
            nameText.text = mutation.Name;
        }

        /// <summary>
        /// Reset the Mutation Card data
        /// </summary>
        public void ResetData()
        {
            mutation = null;
            nameText.text = string.Empty;
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
        
        public void OnPointerEnter(PointerEventData eventData) => Select();
        public void OnPointerExit(PointerEventData eventData) => Deselect();
    }
}
