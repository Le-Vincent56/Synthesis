using Synthesis.Weather;
using System.Collections.Generic;

namespace Synthesis.EventBus.Events.Weather
{
    public struct ClearWeather : IEvent { }
    
    public struct StartDrought : IEvent { }

    public struct StartTorrent : IEvent { }
    
    public struct UpdateWeather : IEvent { }
    public struct WeatherUpdated : IEvent
    {
        public List<WeatherPeriod> WeatherPeriods;
    }

    public struct UpdateWeatherDuration : IEvent
    {
        public WeatherType Type;
        public int Duration;
    }
}
