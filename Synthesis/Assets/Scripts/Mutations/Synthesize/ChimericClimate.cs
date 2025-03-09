using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;

namespace Synthesis.Mutations.Synthesis
{
    public class ChimericClimate : MutationStrategy
    {
        public ChimericClimate()
        {
            name = "Chimeric Climate";
            description = "When you choose a Mutation, there's a 20% chance to shift the weather";
            mutationType = MutationType.Passive;

            partType = MutationPartType.Synthesis;
            color0 = new UnityEngine.Color(0.0f, 0.0f, 0.0f, 1.0f);
            color1 = new UnityEngine.Color(0.0f, 0.0f, 0.0f, 1.0f);
            color2 = new UnityEngine.Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        /// <summary>
        /// When you choose a Mutation, there's a 20% chance to shift the weather
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Calculate the chance to shift
            float chanceToShift = UnityEngine.Random.Range(0.0f, 1.0f);

            // Exit case - the random value is outside the range
            if (chanceToShift > 0.2f) return;

            // Shift the weather
            weather.ShiftWeather();
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new ChimericClimate();
        }
    }
}
