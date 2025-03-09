using Synthesis.EventBus;
using Synthesis.EventBus.Events.Weather;
using System;
using UnityEngine;

namespace Synthesis.Weather
{
    [Serializable]
    public abstract class WeatherType
    {
        [SerializeField] protected int turnsSinceStart;
        [SerializeField] protected int numberOfInfectsSinceStart;
        [SerializeField] protected int duration;

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

        public int Duration
        {
            get => duration;
            set => duration = value;
        }

        /// <summary>
        /// Start the Weather effect
        /// </summary>
        public abstract void StartWeather();

        /// <summary>
        /// Set the initial duration of the Weather
        /// </summary>
        public void SetDuration(int duration) => this.duration = duration;

        /// <summary>
        /// Add to the duration of the Weather
        /// </summary>
        public void AddDuration(int duration)
        {
            this.duration += duration;

            EventBus<UpdateWeatherDuration>.Raise(new UpdateWeatherDuration()
            {
                Type = this,
                Duration = this.duration
            });
        }

        /// <summary>
        /// Tick the Weather
        /// </summary>
        /// <returns>True if the Weather has expired, false if not</returns>
        public bool Tick()
        {
            // Handle variables
            turnsSinceStart++;
            duration--;

            // Exit case - if the duration of the Weather has run out
            if (duration <= 0) return true;

            return false;
        }
    }
}
