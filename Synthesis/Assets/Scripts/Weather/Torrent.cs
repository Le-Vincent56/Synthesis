using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Weather
{
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
        }
    }
}
