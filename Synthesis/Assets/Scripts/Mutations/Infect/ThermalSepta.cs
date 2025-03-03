using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class ThermalSepta : MutationStrategy
    {
        private float localBCRIncrease;

        public ThermalSepta()
        {
            // Set properties
            name = "Thermal Septa";
            description = "In Drought, your base Combat Rating increases by +4% per turn. This bonus rests if Torrent starts.";
            partType = MutationPartType.InfectDrought;
            color0 = new Color(0.95f, 0.8f, 0,1);
            color1 = new Color(0.9f, 0.5f, 0.4f, 1);
            color2 = new Color(1f, 0.5f, 0f, 1f);

            // Set the local base Combat Rating increase to 0
            localBCRIncrease = 0f;
        }

        /// <summary>
        /// In Drought, your base Combat Rating increases by +4% per turn. This bonus rests if Torrent starts.
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - if the current weather is not Drought
            if (weather.CurrentWeather is not Drought drought)
            {
                // Check if the current weather is Torrent
                if(weather.CurrentWeather is Torrent)
                    // Reset the local base Combat Rating increase
                    localBCRIncrease = 0f;

                return;
            }

            // Increase the base Combat Rating by 4% per turn
            calculator.IncreaseBaseAdditives(localBCRIncrease);
        }
    }
}
