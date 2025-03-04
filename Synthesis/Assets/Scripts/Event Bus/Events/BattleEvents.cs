namespace Synthesis.EventBus.Events.Battle
{
    public struct BattleMetricsSet : IEvent
    {
        public int CurrentCombatRating;
        public int CurrentWilt;
        public int TargetCombatRating;
        public int TotalWilt;
    }

    public struct CombatRatingCalculated : IEvent
    {
        public int CombatRating;
    }

    public struct CombatRatingFinalized : IEvent
    {
        public int CombatRating;
    }

    public struct ApplyWilt : IEvent
    {
        public int WiltToApply;
    }

    public struct WiltApplied : IEvent
    {
        public int CurrentWilt;
        public int TotalWilt;
    }

    public struct Wilted : IEvent { }
}
