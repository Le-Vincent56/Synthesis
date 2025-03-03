using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;

namespace Synthesis.Weather
{
    public class Clear : WeatherType
    {
        /// <summary>
        /// Start the Clear Weather
        /// </summary>
        public override void StartWeather()
        {
            // Restart the number of turns since the start of the Clear Weather
            turnsSinceStart = 0;

            // Restart the number of infects since the start of the Clear Weather
            numberOfInfectsSinceStart = 0;
            
            EventBus<ClearWeather>.Raise(new ClearWeather());
        }
    }
}
