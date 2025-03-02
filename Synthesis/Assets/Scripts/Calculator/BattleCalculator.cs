using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Calculator
{
    public class BattleCalculator : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float baseCombatRating = 10f;
        [SerializeField] private float bcrAdditives;
        [SerializeField] private float bcrMultipliers;
        [SerializeField] private float fcrAdditives;
        [SerializeField] private float incomingWiltPercentage = 1f;
        [SerializeField] private float bcrPermanentAdditives = 0f;

        [Header("Infect")]
        [SerializeField] private int consecutiveInfects = 0;

        public int ConsecutiveInfects { get => consecutiveInfects; }

        /// <summary>
        /// Increase the base Combat Rating by an additive percentage
        /// </summary>
        public void IncreaseBaseAdditives(float bcrAdditive) => bcrAdditives += bcrAdditive;

        /// <summary>
        /// Increase the base Combat Rating by a multiplicative percentage
        /// </summary>
        public void IncreaseBaseMultipliers(float bcrMultiplier) => bcrMultipliers += bcrMultiplier;

        /// <summary>
        /// Increase the final Combat Rating by an additive percentage
        /// </summary>
        public void IncreaseFinalAdditives(float fcrAdditive) => fcrAdditives += fcrAdditive;

        /// <summary>
        /// Increase the base combat rating permanently
        /// </summary>
        public void IncreaseBasePermenentAdditive(float bcrPermanentAdditive) => bcrPermanentAdditives += bcrPermanentAdditive;

    }
}
