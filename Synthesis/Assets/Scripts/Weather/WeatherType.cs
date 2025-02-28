namespace Synthesis.Weather
{
    public abstract class WeatherType
    {
        protected int numberOfInfectsSinceStart;

        public int NumberOfInfectsSinceStart 
        { 
            get => numberOfInfectsSinceStart; 
            set => numberOfInfectsSinceStart = value; 
        }

        /// <summary>
        /// Start the Weather effect
        /// </summary>
        public abstract void StartWeather();
    }
}
