using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Weather
{
    public enum WeatherType
    {
        Clear,
        Drought,
        Torrent,
    }

    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private WeatherType currentWeather;
        private int turnsUntilWeatherChange;
        private Dictionary<WeatherType, float> weatherPercentages;

        public WeatherType CurrentWeather { get => currentWeather; }

        private void Awake()
        {
            weatherPercentages = new Dictionary<WeatherType, float>
            {
                { WeatherType.Drought, 0.5f },
                { WeatherType.Torrent, 0.5f }
            };

            // Set clear weather to begin wtih
            currentWeather = WeatherType.Clear;
        }

        /// <summary>
        /// Update the Weather System
        /// </summary>
        public void UpdateWeather()
        {
            // Check if there are still turns until the weather change
            if(turnsUntilWeatherChange > 0)
            {
                // Tick down the amount of turns unti the weather change
                turnsUntilWeatherChange--;

                return;
            }

            // If there are no turns left until the weather change, change the weather
            currentWeather = ChooseWeather();

            // Set a new Weather date
            SetWeatherDate();
        }

        /// <summary>
        /// Activate the current Weather effect
        /// </summary>
        public void ActivateWeatherEffect()
        {
            // Check the current Weather
            switch (currentWeather)
            {
                case WeatherType.Clear:
                    // Clear weather has no effect
                    break;
                case WeatherType.Drought:
                    break;
                case WeatherType.Torrent:
                    break;
            }
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
            foreach(KeyValuePair<WeatherType, float> weather in weatherPercentages)
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
            return WeatherType.Drought;
        }
    }
}
