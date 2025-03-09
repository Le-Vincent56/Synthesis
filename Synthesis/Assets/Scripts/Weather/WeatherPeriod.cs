using System;
using UnityEngine;

namespace Synthesis.Weather
{
    [Serializable]
    public class WeatherPeriod
    {
        [SerializeField] public WeatherType WeatherType;
        [SerializeField] public int Duration;

        public WeatherPeriod(WeatherType type, int duration)
        {
            WeatherType = type;
            Duration = duration;
        }

        public void Debug(int index)
        {
            UnityEngine.Debug.Log($"[{index}] Weather Period: {WeatherType} for {Duration} ticks");
        }
    }
}
