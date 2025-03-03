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

    public struct UpdateTurns : IEvent
    {
        public int CurrentTurn;
        public int TotalTurns;
    }

    public struct HideTurnHeader : IEvent { }

    public struct ShowPlayerInfo : IEvent { }

    public struct HidePlayerInfo : IEvent { }
    public struct ShowSynthesizeShop : IEvent { }
    public struct HideSynthesizeShop : IEvent { }
}
