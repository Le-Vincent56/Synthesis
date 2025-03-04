using Synthesis.EventBus.Events.Creatures;
using Synthesis.EventBus;
using UnityEngine;
using DG.Tweening;

namespace Synthesis.Creatures
{
    public class Enemy : MonoBehaviour
    {
        [Header("Tweening Variables")]
        [SerializeField] private float jumpPower = 1f;
        [SerializeField] private float jumpDuration = 0.5f;
        private Tween jumpTween;

        private EventBinding<EnemyAttack> onEnemyAttack;

        private void OnEnable()
        {
            onEnemyAttack = new EventBinding<EnemyAttack>(PlayAttack);
            EventBus<EnemyAttack>.Register(onEnemyAttack);
        }

        private void OnDisable()
        {
            EventBus<EnemyAttack>.Deregister(onEnemyAttack);
        }

        private void OnDestroy()
        {
            // Kill the jump tween if it exists
            jumpTween?.Kill();
        }

        private void PlayAttack()
        {
            // Get the jump positions
            float toY = transform.localPosition.y + jumpPower;
            float fromY = transform.localPosition.y;

            // Activate the jump
            Jump(toY, jumpDuration / 2f, () => Jump(fromY, jumpDuration / 2f));
        }

        /// <summary>
        /// Jump the player
        /// </summary>
        private void Jump(float endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill the jump tween if it exists
            jumpTween?.Kill();

            // Create a new jump tween
            jumpTween = transform.DOLocalMoveY(endValue, duration);

            // Set the easing
            jumpTween.SetEase(Ease.OutQuad);

            // Exit case - if there is no completion action
            if (onComplete == null) return;

            // Set the completion action
            jumpTween.onComplete += onComplete;
        }
    }
}
