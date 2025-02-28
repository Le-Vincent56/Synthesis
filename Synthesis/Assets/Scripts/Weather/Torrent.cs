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
            // Restart the number of infects since the start of the Torrent
            numberOfInfectsSinceStart = 0;
        }
    }
}
