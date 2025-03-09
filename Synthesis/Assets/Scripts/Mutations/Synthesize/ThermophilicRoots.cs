using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;

namespace Synthesis.Mutations.Synthesis
{
    public class ThermophilicRoots : MutationStrategy
    {
        public ThermophilicRoots()
        {
            name = "Thermophilic Roots";
            description = "When you choose a Mutation in Drought, gain +3% stacking base Combat Rating permanently";
            mutationType = MutationType.Passive;

            partType = MutationPartType.SynthesisDrought;
        }

        /// <summary>
        /// When you choose a Mutation in Drought, gain +3% stacking base Combat Rating permanently
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - if the current weather is not Drought
            if (weather.CurrentWeather is not Drought drought) return;

            // Increase the base Combat Rating permanently by 3%
            calculator.IncreaseBasePermanentPercentage(0.03f);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new ThermophilicRoots();
        }
    }
}
