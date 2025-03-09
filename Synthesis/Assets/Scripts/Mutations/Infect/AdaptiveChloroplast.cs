using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class AdaptiveChloroplast : MutationStrategy
    {
        public AdaptiveChloroplast()
        {
            // Set properties
            name = "Adaptive Chloroplast";
            description = "After choosing a Mutation, the next Infect has a 15% higher base Combat Rating";
            mutationType = MutationType.Active;
            partType = MutationPartType.Infect;
            color0 = new Color(1.0f, 0.9f, 0.0f,1);
            color1 = new Color(0.0f, 1.0f, 0.0f, 1);
        }

        /// <summary>
        /// After choosing a Mutation, the next Infect has a 15% higher base Combat Rating
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - the last Action was Infect
            if (calculator.LastAction == Action.Infect) return;

            // Increase the base Combat Rating by 15%
            calculator.IncreaseBaseAdditives(0.15f);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new AdaptiveChloroplast();
        }
    }
}
