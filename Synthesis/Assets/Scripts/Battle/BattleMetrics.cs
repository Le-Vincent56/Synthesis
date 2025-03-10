using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;
using Synthesis.EventBus.Events.Turns;
using Synthesis.ServiceLocators;
using UnityEngine;

namespace Synthesis.Battle
{
    public class BattleMetrics : MonoBehaviour
    {
        [Header("Scaling")]
        [SerializeField] private int difficultyLevel;
        [SerializeField] private float ratingLevelPercentageIncrease = 0.50f; // 20% increase per level
        [SerializeField] private float wiltPerLevelPercentageIncrease = 0.10f; // 10% increase per level

        [Header("Fester")]
        [SerializeField] private int baseFester = 20;
        [SerializeField] private int currentFester;
        [SerializeField] private int targetFester;

        [Header("Wilt")]
        [SerializeField] private int baseTotalWilt = 50;
        [SerializeField] private int currentWilt;
        [SerializeField] private int totalWilt;

        private EventBinding<StartBattle> onStartBattle;
        private EventBinding<FesterFinalized> onCombatRatingFinalized;
        private EventBinding<ApplyWilt> onApplyWilt;

        public int CurrentFester { get => currentFester; }
        public int TargetFester { get => targetFester; }

        private void Awake()
        {
            // Set the initial difficulty level
            difficultyLevel = 0;

            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void OnEnable()
        {
            onStartBattle = new EventBinding<StartBattle>(SetMetrics);
            EventBus<StartBattle>.Register(onStartBattle);

            onCombatRatingFinalized = new EventBinding<FesterFinalized>(UpdateCombatRating);
            EventBus<FesterFinalized>.Register(onCombatRatingFinalized);

            onApplyWilt = new EventBinding<ApplyWilt>(UpdateWilt);
            EventBus<ApplyWilt>.Register(onApplyWilt);
        }

        private void OnDisable()
        {
            EventBus<StartBattle>.Deregister(onStartBattle);
            EventBus<FesterFinalized>.Deregister(onCombatRatingFinalized);
            EventBus<ApplyWilt>.Deregister(onApplyWilt);
        }

        /// <summary>
        /// Set the Mmtrics at the start of a battle
        /// </summary>
        private void SetMetrics()
        {
            // Set the current values
            currentFester = 0;
            currentWilt = 0;

            // Calculate the target combat rating
            CalculateTargetCombatRating();

            // Calculate the total wilt
            CalculateTotalWilt();

            // Publish the metrics
            EventBus<BattleMetricsSet>.Raise(new BattleMetricsSet
            {
                CurrentFester = currentFester,
                CurrentWilt = currentWilt,
                TargetFester = targetFester,
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
            targetFester = Mathf.RoundToInt(baseFester + (baseFester * ratingIncrease));
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
        private void UpdateCombatRating(FesterFinalized eventData)
        {
            // Update the current combat rating
            currentFester += eventData.CalculatedFester;

            // Check if the current Combat Rating is greater than or equal to the target Combat Rating
            if (currentFester >= targetFester)
            {
                // Increase the difficulty level
                difficultyLevel++;

                // Move to the next battle
                EventBus<WinBattle>.Raise(new WinBattle());

                return;
            }

            EventBus<FesterFinished>.Raise(new FesterFinished());
        }

        /// <summary>
        /// Update the current Player Damage
        /// </summary>
        public void UpdateWilt(ApplyWilt eventData)
        {
            // Add to the current Wilt
            currentWilt += eventData.WiltToApply;

            // Check if the current player damage is greater than or equal to the total player damage
            if (currentWilt >= totalWilt)
            {
                currentWilt = 0;

                // Set Wilted
                EventBus<Wilted>.Raise(new Wilted());
            }

            // Publish the applied wilt event
            EventBus<WiltApplied>.Raise(new WiltApplied() { CurrentWilt = currentWilt, TotalWilt = totalWilt });
        }
    }
}
