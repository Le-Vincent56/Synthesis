namespace Synthesis.EventBus.Events.UI
{
    public struct SetInfoText : IEvent
    {
        public string Text;
    }

    public struct ShowTurnHeader : IEvent
    {
        public string Text;
    }

    public struct HideTurnHeader : IEvent { }

    public struct ShowPlayerInfo : IEvent { }

    public struct HidePlayerInfo : IEvent { }
    public struct ShowSynthesizeShop : IEvent { }
    public struct HideSynthesizeShop : IEvent { }
}
