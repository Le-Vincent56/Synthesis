using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Calculator
{
    public class BattleCalculator : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float baseCombatRating = 10f;
        [SerializeField] private float baseCombatRatingAdditives;
        [SerializeField] private float baseCombatRatingMultipliers;
        [SerializeField] private float finalCombatRatingAdditives;
        [SerializeField] private float incomingWiltPercentage = 1f;

        public void IncreaseBaseAdditives(float bcrAdditive) => baseCombatRatingAdditives += bcrAdditive;
        public void IncreaseBaseMultipliers(float bcrMultiplier) => baseCombatRatingMultipliers += bcrMultiplier;
        public void IncreaseFinalAdditives(float fcrAdditive) => finalCombatRatingAdditives += fcrAdditive;

    }
}
