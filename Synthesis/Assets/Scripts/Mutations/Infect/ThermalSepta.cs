using Synthesis.Calculator;
using Synthesis.Weather;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class ThermalSepta : MutationStrategy
    {
        private float localBCRIncrease;

        public ThermalSepta()
        {
            // Set the local base Combat Rating increase to 0
            localBCRIncrease = 0f;
        }

        /// <summary>
        /// In Drought, your base Combat Rating increases by +4% per turn. This bonus rests if Torrent starts.
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather)
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
