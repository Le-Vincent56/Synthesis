namespace Synthesis.Weather
{
    public class Clear : WeatherType
    {
        /// <summary>
        /// Start the Clear Weather
        /// </summary>
        public override void StartWeather()
        {
            // Restart the number of infects since the start of the Clear Weather
            numberOfInfectsSinceStart = 0;
        }
    }
}
