using Synthesis.Battle;
using Synthesis.Weather;

namespace Synthesis.Mutations.Infect
{
    public class AdaptiveChloroplast : MutationStrategy
    {
        public AdaptiveChloroplast()
        {
            // Set properties
            name = "Adaptive Chloroplast";
            description = "After choosing a Mutation, the next Infect has a 15% higher base Combat Rating";
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
    }
}
