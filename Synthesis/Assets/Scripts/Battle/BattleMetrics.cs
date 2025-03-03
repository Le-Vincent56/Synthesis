using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;
using Synthesis.EventBus.Events.Turns;
using UnityEngine;

namespace Synthesis.Battle
{
    public class BattleMetrics : MonoBehaviour
    {
        [Header("Scaling")]
        [SerializeField] private int difficultyLevel;
        [SerializeField] private float ratingLevelPercentageIncrease = 0.20f; // 20% increase per level
        [SerializeField] private float wiltPerLevelPercentageIncrease = 0.30f; // 30% increase per level

        [Header("Combat Rating")]
        [SerializeField] private int baseTargetCombatRating = 20;
        [SerializeField] private int currentCombatRating;
        [SerializeField] private int targetCombatRating;

        [Header("Wilt")]
        [SerializeField] private int baseTotalWilt = 50;
        [SerializeField] private int currentWilt;
        [SerializeField] private int totalWilt;

        private EventBinding<StartBattle> onStartBattle;
        private EventBinding<CombatRatingCalculated> onCombatRatingCalculated;

        private void Awake()
        {
            // Set the initial difficulty level
            difficultyLevel = 0;
        }

        private void OnEnable()
        {
            onStartBattle = new EventBinding<StartBattle>(SetMetrics);
            EventBus<StartBattle>.Register(onStartBattle);

            onCombatRatingCalculated = new EventBinding<CombatRatingCalculated>(UpdateCombatRating);
            EventBus<CombatRatingCalculated>.Register(onCombatRatingCalculated);
        }

        private void OnDisable()
        {
            EventBus<StartBattle>.Deregister(onStartBattle);
            EventBus<CombatRatingCalculated>.Deregister(onCombatRatingCalculated);
        }

        /// <summary>
        /// Set the Mmtrics at the start of a battle
        /// </summary>
        private void SetMetrics()
        {
            // Set the current values
            currentCombatRating = 0;
            currentWilt = 0;

            // Calculate the target combat rating
            CalculateTargetCombatRating();

            // Calculate the total wilt
            CalculateTotalWilt();

            // Publish the metrics
            EventBus<BattleMetricsSet>.Raise(new BattleMetricsSet
            {
                CurrentCombatRating = currentCombatRating,
                CurrentWilt = currentWilt,
                TargetCombatRating = targetCombatRating,
                TotalWilt = totalWilt
            });
        }


        /// <summary>
        /// Calculate the Target Combat Rating
        /// </summary>
        private void CalculateTargetCombatRating()
        {
            // Calculate the rating increase based on the difficulty level
            float ratingIncrease = ratingLevelPercentageIncrease * difficultyLevel;

            // Calculate the new target combat rating
            targetCombatRating = Mathf.RoundToInt(baseTargetCombatRating + (baseTargetCombatRating * ratingIncrease));
        }

        /// <summary>
        /// Calculate the Total Wilt
        /// </summary>
        private void CalculateTotalWilt()
        {
            // Calculate the wilt increase based on the difficulty level
            float wiltIncrease = wiltPerLevelPercentageIncrease * difficultyLevel;

            // Calculate the new total wilt
            totalWilt = Mathf.RoundToInt(baseTotalWilt + (baseTotalWilt * wiltIncrease));
        }

        /// <summary>
        /// Update the current Combat Rating
        /// </summary>
        private void UpdateCombatRating(CombatRatingCalculated eventData)
        {
            // Update the current combat rating
            currentCombatRating += eventData.CombatRating;

            // Check if the current Combat Rating is greater than or equal to the target Combat Rating
            if (currentCombatRating >= targetCombatRating)
            {
                // TODO: Move to the next battle
            }

            // Publish the finalized Combat Rating
            EventBus<CombatRatingFinalized>.Raise(new CombatRatingFinalized { CombatRating = currentCombatRating });
        }

        /// <summary>
        /// Update the current Player Damage
        /// </summary>
        public void UpdateWilt(int calcualtedWilt)
        {
            currentWilt += calcualtedWilt;

            // Check if the current player damage is greater than or equal to the total player damage
            if (currentWilt >= totalWilt)
            {
                // TODO: Lose a turn
            }
        }
    }
}
