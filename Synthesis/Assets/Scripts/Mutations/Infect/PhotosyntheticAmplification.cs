using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class PhotosyntheticAmplification : MutationStrategy
    {
        public PhotosyntheticAmplification()
        {
            // Set properties
            name = "Photosynthetic Amplification";
            description = "The first Infect in Drought has a 25% higher final Combat Rating";

            partType = MutationPartType.InfectDrought;
            color0 = new Color(0.9f, 0.1f, 0.1f,1);
            color1 = new Color(0.9f, 0.9f, 0.2f, 1);
            color2 = new Color(1f, 0.5f, 0f, 1f);
        }

        /// <summary>
        /// The first Infect in Drought has a 25% higher final Combat Rating
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
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
