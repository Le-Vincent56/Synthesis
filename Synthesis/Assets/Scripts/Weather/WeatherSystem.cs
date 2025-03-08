using System.Collections.Generic;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;
using Synthesis.ServiceLocators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Synthesis.Weather
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private WeatherType currentWeather;
        private int turnsUntilWeatherChange;
        private Dictionary<WeatherType, float> weatherPercentages;

        private EventBinding<UpdateWeather> onUpdateWeather;

        public WeatherType CurrentWeather { get => currentWeather; }

        public static readonly WeatherType Clear = new Clear();
        public static readonly WeatherType Drought = new Drought();
        public static readonly WeatherType Torrent = new Torrent();

        private void Awake()
        {
            // Initialize the Weather Percentages
            InitializeWeatherPercentages();

            // Set clear weather to begin wtih
            currentWeather = Clear;

            // Register this as service
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void OnEnable()
        {
            onUpdateWeather = new EventBinding<UpdateWeather>(UpdateWeather);
            EventBus<UpdateWeather>.Register(onUpdateWeather);
        }

        private void OnDisable()
        {
            EventBus<UpdateWeather>.Deregister(onUpdateWeather);
        }

        private void InitializeWeatherPercentages()
        {
            weatherPercentages = new Dictionary<WeatherType, float>
            {
                { Clear, 0.50f },
                { Drought, 0.25f },
                { Torrent, 0.25f }
            };
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
            // Generate a random value between 0 and 1
            float randomValue = Random.Range(0f, 1f);
            float cumulative = 0f;

            // Iterate through each WeatherType and its corresponding percentage
            foreach (KeyValuePair<WeatherType, float> weather in weatherPercentages)
            {
                // Add the current weather's percentage to the cumulative total
                cumulative += weather.Value;

                // If the random value is less than or equal to the cumulative total, select this weather
                if (randomValue <= cumulative)
                    return weather.Key;
            }

            // Fall back on the Clear weather if no weather was chosen
            return Clear;
        }

        /// <summary>
        /// Adjust weather percentages
        /// </summary>
        public void AdjustWeatherPercentages(WeatherType weatherType, float percentageIncrease)
        {
            // Exit case - if the Weather Type does not exist in the dictionary
            if (!weatherPercentages.ContainsKey(weatherType))
                return;

            // Calculate the current total of all weather percentages
            float currentTotal = 0f;

            // Iterate through each key-value pair
            foreach (KeyValuePair<WeatherType, float> kvp in weatherPercentages)
                currentTotal += kvp.Value;

            // Calculate the available adjustment room
            float availableAdjustment = 1f - currentTotal + weatherPercentages[weatherType];

            // Ensure the increase doesn't exceed the available adjustment room
            float newWeight = Mathf.Clamp(weatherPercentages[weatherType] + percentageIncrease, 0f, availableAdjustment);
            weatherPercentages[weatherType] = newWeight;

            // Normalize the rest of the weights to ensure the total sum equals 1
            NormalizeWeatherPercentages();
        }

        /// <summary>
        /// Ensures that the total probability always sums to 1
        /// </summary>
        private void NormalizeWeatherPercentages()
        {
            // Calculate the total sum of all weather percentages
            float total = 0f;
            foreach (KeyValuePair<WeatherType, float> kvp in weatherPercentages)
                total += kvp.Value;

            // Normalize each weather percentage so that the total sum equals 1
            foreach (WeatherType key in weatherPercentages.Keys)
                weatherPercentages[key] /= total;
        }
    }
}
