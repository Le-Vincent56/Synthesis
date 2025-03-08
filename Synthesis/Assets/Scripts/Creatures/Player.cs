using System;
using System.Collections;
using DG.Tweening;
using Synthesis.Creatures.Visual;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Creatures;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Mutations;
using Synthesis.ServiceLocators;
using UnityEngine;

namespace Synthesis.Creatures
{
    public enum MutationPartType
    {
        Infect = 0,
        Synthesis = 1,
        InfectDrought = 2,
        InfectTorrent = 4,
        SynthesisDrought = 3,
        SynthesisTorrent = 5,
        // TODO: Make actual weather variations
    }
    
    public class Player : MonoBehaviour
    {
        [SerializeField] private CreaturePiece[] availablePieces;
        
        [SerializeField] private CreaturePiece piece;
        private static readonly int Color1 = Shader.PropertyToID("_Color1");

        private EventBinding<Synthesize> onSynthesize;
        private EventBinding<PlayerAttack> onPlayerAttack;

        [Header("Tweening Variables")]
        [SerializeField] private float jumpPower = 1f;
        [SerializeField] private float jumpDuration = 0.5f;
        private Tween jumpTween;

        private CameraController cameraController;
        
        private EventBinding<PlayerHit> onPlayerHit;

        public CreaturePiece Piece { get => piece; }

        private void Start()
        {
            cameraController = ServiceLocator.ForSceneOf(this).Get<CameraController>();
        }

        // Methods
        private void OnEnable()
        {
            onSynthesize = new EventBinding<Synthesize>(OnSynthesize);
            EventBus<Synthesize>.Register(onSynthesize);

            onPlayerAttack = new EventBinding<PlayerAttack>(PlayAttack);
            EventBus<PlayerAttack>.Register(onPlayerAttack);
            
            onPlayerHit = new EventBinding<PlayerHit>(StartDamageBuffer);
            EventBus<PlayerHit>.Register(onPlayerHit);
        }

        private void OnDisable()
        {
            EventBus<Synthesize>.Deregister(onSynthesize);
            EventBus<PlayerAttack>.Deregister(onPlayerAttack);
        }

        private void OnDestroy()
        {
            // Kill the jump tween if it exists
            jumpTween?.Kill();
        }

        private void OnSynthesize(Synthesize eventData)
        {
            UpdateCreatureVisual(eventData.Mutation);
        }

        private void PlayAttack()
        {
            // Get the jump positions
            float toY = transform.localPosition.y + jumpPower;
            float fromY = transform.localPosition.y;

            // Activate the jump
            Jump(toY, jumpDuration / 2f, () => 
            { 
                cameraController.GenerateImpulse();
                Jump(fromY, jumpDuration / 2f);
            });
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
        
        private void StartDamageBuffer(PlayerHit eventData)
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
                piece.primaryColorIn[0].color = Color.red;
                yield return new WaitForSeconds(0.1f);
                piece.primaryColorIn[0].color = Color.clear;
                yield return new WaitForSeconds(0.1f);
                hitBuffer -= 0.2f;
            }

            // remove player buffering and also make sprite white again.
            piece.primaryColorIn[0].color = Color.white;
            //StopCoroutine(DamageGrace());
        }

        /// <summary>
        /// Updates creature based on mutation added.
        /// </summary>
        /// <param name="trait"></param>
        private void UpdateCreatureVisual(MutationStrategy mutation)
        {
            foreach (var connector in piece.connectors)
            {
                var con = connector;
                var oldPiece = piece;
                while (con.child)
                {
                    oldPiece = con.child;
                    con = con.child.connectors[0];
                }
                var newPiece = Instantiate(availablePieces[(int)mutation.PartType]);
                newPiece.transform.position = (con.transform.position + new Vector3(0, 0, 0.01f));
                newPiece.transform.parent = con.transform;
                Color bleedColor = Color.clear;
                float bleedrate = 0.0f;
                if (oldPiece.primaryColorIn != null && newPiece.primaryColorIn.Length > 0 && oldPiece.primaryColorIn[0] != null)
                {
                    bleedColor = oldPiece.primaryColorIn[0].material.GetColor(Color1);
                    bleedrate = 0.4f;
                }
                
                newPiece.SetMutation(mutation, bleedColor, bleedrate);
                
                con.child = newPiece;
            }
        }
    }
}
