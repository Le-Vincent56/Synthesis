namespace Synthesis.EventBus.Events.Weather
{
    public struct ClearWeather : IEvent { }
    
    public struct StartDrought : IEvent { }

    public struct StartTorrent : IEvent { }
    
    public struct UpdateWeather : IEvent { }
}
