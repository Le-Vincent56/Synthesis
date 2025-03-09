using Synthesis.Battle;
using Synthesis.Weather;

namespace Synthesis.Mutations.Infect
{
    public class RecursiveSpores : MutationStrategy
    {
        public RecursiveSpores()
        {
            // Set properties
            name = "Recursive Spores";
            description = "Increase the final Combat Rating by +6% per Mutation";
            mutationType = MutationType.Active;
        }

        /// <summary>
        /// Increase the final Combat Rating by +6% per Mutation
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Get the number of Mutations the player has
            int numberOfMutations = mutations.Mutations.Count;

            // Increase the final Combat Rating by 6% per Mutation
            calculator.IncreaseFinalAdditives(0.06f * numberOfMutations);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new RecursiveSpores();
        }
    }
}
