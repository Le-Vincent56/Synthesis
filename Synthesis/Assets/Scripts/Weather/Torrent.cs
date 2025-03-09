using System;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;

namespace Synthesis.Weather
{
    [Serializable]
    public class Torrent : WeatherType
    {
        /// <summary>
        /// Start the Torrent Weather
        /// </summary>
        public override void StartWeather()
        {
            // Restart the number of turns since the start of the Torrent
            turnsSinceStart = 0;

            // Restart the number of infects since the start of the Torrent
            numberOfInfectsSinceStart = 0;
            
            EventBus<StartTorrent>.Raise(new StartTorrent());
        }
    }
}
