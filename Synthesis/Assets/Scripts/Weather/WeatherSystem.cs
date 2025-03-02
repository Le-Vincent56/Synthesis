using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Weather
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private WeatherType currentWeather;
        private Clear clearWeather;
        private int turnsUntilWeatherChange;
        private Dictionary<WeatherType, float> weatherPercentages;

        public WeatherType CurrentWeather { get => currentWeather; }

        private void Awake()
        {
            Clear clear = new Clear();
            Drought drought = new Drought();
            Torrent torrent = new Torrent();

            weatherPercentages = new Dictionary<WeatherType, float>
            {
                { drought, 0.5f },
                { torrent, 0.5f }
            };

            // Set clear weather to begin wtih
            currentWeather = clear;
        }

        /// <summary>
        /// Update the Weather System
        /// </summary>
        public void UpdateWeather()
        {
            // Check if there are still turns until the weather change
            if (turnsUntilWeatherChange > 0)
            {
                // Tick down the amount of turns unti the weather change
                turnsUntilWeatherChange--;

                return;
            }

            // If there are no turns left until the weather change, change the weather
            currentWeather = ChooseWeather();

            // Set a new Weather date
            SetWeatherDate();

            // Start the weather effect
            currentWeather.StartWeather();
        }

        /// <summary>
        /// Set how many turns until the next Weather change
        /// </summary>
        private void SetWeatherDate() => turnsUntilWeatherChange = Random.Range(3, 7);

        /// <summary>
        /// Choose the current Weather given the weights of the WeatherPercentages
        /// </summary>
        private WeatherType ChooseWeather()
        {
            // Create a counter for the total weight
            float totalWeight = 0f;

            // Iterate through each KeyValuePair
            foreach (KeyValuePair<WeatherType, float> weather in weatherPercentages)
            {
                // Add the weight to the total weight
                totalWeight += weather.Value;
            }

            // Get the random value from the total weight
            float randomValue = Random.Range(0f, totalWeight);

            // Start tracking a current sum
            float currentSum = 0f;

            // Iterate through each KeyValuePair
            foreach (KeyValuePair<WeatherType, float> weather in weatherPercentages)
            {
                // Add the current weather value to the current sum
                currentSum += weather.Value;

                // Check If the random value is less than the current sum
                if (randomValue <= currentSum)
                {
                    // Set the current weather to the current key
                    return weather.Key;
                }
            }

            // Fall back on the Drought weather if no weather was chosen
            return clearWeather;
        }
    }
}
