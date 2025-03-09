using System;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;

namespace Synthesis.Weather
{
    [Serializable]
    public class Drought : WeatherType
    {
        /// <summary>
        /// Start the Drought Weather
        /// </summary>
        public override void StartWeather()
        {
            // Restart the number of turns since the start of the Drought
            turnsSinceStart = 0;

            // Restart the number of infects since the start of the Drought
            numberOfInfectsSinceStart = 0;
            
            EventBus<StartDrought>.Raise(new StartDrought());
        }
    }
}
