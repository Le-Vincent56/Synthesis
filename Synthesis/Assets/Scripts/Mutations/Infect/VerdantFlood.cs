using Synthesis.Calculator;
using Synthesis.Weather;
using System;

namespace Synthesis.Mutations.Infect
{
    public class VerdantFlood : MutationStrategy
    {
        public VerdantFlood()
        {
            name = "Verdant Flood";
            description = "In Torrent, every third Infect grants +5 base Combat Rating permanently";
        }

        /// <summary>
        /// In Torrent, every third Infect grants +5 base Combat Rating permanently
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather)
        {
            // Exit case - if the current weather is not Torrent
            if (weather.CurrentWeather is not Torrent torrent) return;

            // Exit case - if there are infects since the start of the Drought
            if (torrent.NumberOfInfectsSinceStart % 3 != 0) return;

            // Increase the base Combat Rating permanently by 5
            calculator.IncreaseBasePermenentAdditive(5f);
        }
    }
}
