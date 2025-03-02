using Synthesis.Calculator;
using Synthesis.Weather;

namespace Synthesis.Mutations.Infect
{
    public class PhotosyntheticAmplification : MutationStrategy
    {
        public override string Name => "Photosynthetic Amplification";
        public override string Description => "The first Infect in Drought has a 25% higher final Combat Rating";

        /// <summary>
        /// The first Infect in Drought has a 25% higher final Combat Rating
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather)
        {
            // Exit case - if the current weather is not Drought
            if (weather.CurrentWeather is not Drought drought) return;

            // Exit case - if there are infects since the start of the Drought
            if (drought.NumberOfInfectsSinceStart > 0) return;

            // Increase the final additives by 25%
            calculator.IncreaseFinalAdditives(0.25f);
        }
    }
}
