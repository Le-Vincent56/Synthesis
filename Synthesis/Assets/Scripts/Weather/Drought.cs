using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Weather
{
    public class Drought : WeatherType
    {
        /// <summary>
        /// Start the Drought Weather
        /// </summary>
        public override void StartWeather()
        {
            // Restart the number of infects since the start of the Drought
            numberOfInfectsSinceStart = 0;
        }
    }
}
