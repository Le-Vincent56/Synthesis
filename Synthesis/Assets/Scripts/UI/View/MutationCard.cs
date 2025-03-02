using DG.Tweening;
using Synthesis.Mutations;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Synthesis.UI.View
{
    public class MutationCard : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, IPointerClickHandler
    {
        private MutationStrategy mutation;
        private RectTransform rectTransform;
        private Text nameText;
        private Text descriptionText;

        [Header("Tweening Variables")]
        [SerializeField] private float scaleDuration;
        [SerializeField] private float scaleAmount;
        [SerializeField] private float translateDuration;
        [SerializeField] private float translateAmount;
        private Tween scaleTween;
        private Tween translateTween;

        public void OnDestroy()
        {
            // Kill any tweens if they exist
            scaleTween?.Kill();
            translateTween?.Kill();
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

        private void OnClick()
        {

        }

        

        private void Highlight()
        {
            // Scale and translate
            Scale(Vector3.one * scaleAmount, scaleDuration);
            Translate(translateAmount, translateDuration);
        }

        private void Unhighlight()
        {
            Scale(Vector3.one, scaleDuration);
            Translate(-translateAmount, translateDuration);
        }

        private void Scale(Vector3 endValue, float duration)
        {
            // Kill the scale tween if it exists
            scaleTween?.Kill();

            // Set the scale tween
            scaleTween = rectTransform.DOScale(endValue, duration);
        }

        private void Translate(float amount, float duration)
        {
            // Kill the translate tween if it exists
            translateTween?.Kill();

            // Set the translate tween
            translateTween = rectTransform.DOAnchorPosY(
                rectTransform.anchoredPosition.y + amount,
                duration
            );
        }

        public void OnSelect(BaseEventData eventData) => Highlight();

        public void OnDeselect(BaseEventData eventData) => Unhighlight();

        public void OnSubmit(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
