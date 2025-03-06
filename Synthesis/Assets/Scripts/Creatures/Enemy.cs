using System.Collections;
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
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Tween jumpTween;

        private EventBinding<EnemyAttack> onEnemyAttack;
        private EventBinding<EnemyHit> onEnemyHit;

        private void OnEnable()
        {
            onEnemyAttack = new EventBinding<EnemyAttack>(PlayAttack);
            EventBus<EnemyAttack>.Register(onEnemyAttack);

            onEnemyHit = new EventBinding<EnemyHit>(StartDamageBugger);
            EventBus<EnemyHit>.Register(onEnemyHit);
        }

        private void OnDisable()
        {
            EventBus<EnemyAttack>.Deregister(onEnemyAttack);
            EventBus<EnemyHit>.Deregister(onEnemyHit);
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

        private void StartDamageBugger(EnemyHit eventData)
        {
            StartCoroutine(DamageBuffer());
        }
        
        /// <summary>
        ///  Invincibility for a short period after taking damage.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageBuffer()
        {
            float hitBuffer = 1.0f;
            while (hitBuffer > 0)
            {
                // flash the player's sprite while invincible.
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = Color.clear;
                yield return new WaitForSeconds(0.1f);
                hitBuffer -= 0.2f;
            }

            // remove player buffering and also make sprite white again.
            spriteRenderer.color = Color.white;
            //StopCoroutine(DamageGrace());
        }
    }
}
