using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Calculator
{
    public class BattleCalculator : MonoBehaviour
    {
        [SerializeField] private float baseCombatRating = 10f;
        [SerializeField] private float baseCombatRatingAdditives;
        [SerializeField] private float baseCombatRatingMultipliers;
        [SerializeField] private float incomingWiltPercentage = 1f;
    }
}
