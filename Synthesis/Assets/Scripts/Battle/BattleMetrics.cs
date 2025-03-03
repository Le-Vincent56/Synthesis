using UnityEngine;

namespace Synthesis.Battle
{
    public class BattleMetrics : MonoBehaviour
    {
        [Header("Combat Rating")]
        [SerializeField] private int currentCombatRating;
        [SerializeField] private int targetCombatRating;

        [Header("Player Damage")]
        [SerializeField] private int currentPlayerDamage;
        [SerializeField] private int totalPlayerDamage;

        /// <summary>
        /// Update the current Combat Rating
        /// </summary>
        public void UpdateCombatRating(int calculatedCombatRating)
        {
            // Update the current combat rating
            currentCombatRating += calculatedCombatRating;

            // Check if the current Combat Rating is greater than or equal to the target Combat Rating
            if (currentCombatRating >= targetCombatRating)
            {
                // TODO: Move to the next battle
            }
        }

        /// <summary>
        /// Update the current Player Damage
        /// </summary>
        public void UpdatePlayerDamage(int calculatedPlayerDamage)
        {
            currentPlayerDamage += calculatedPlayerDamage;

            // Check if the current player damage is greater than or equal to the total player damage
            if (currentPlayerDamage >= totalPlayerDamage)
            {
                // TODO: Lose a turn
            }
        }
    }
}
