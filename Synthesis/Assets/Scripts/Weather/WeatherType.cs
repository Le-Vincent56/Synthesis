namespace Synthesis.Weather
{
    public abstract class WeatherType
    {
        protected int turnsSinceStart;
        protected int numberOfInfectsSinceStart;

        public int TurnsSinceStart 
        { 
            get => turnsSinceStart; 
            set => turnsSinceStart = value; 
        }

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
