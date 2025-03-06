namespace Synthesis.EventBus.Events.Battle
{
    public struct BattleMetricsSet : IEvent
    {
        public int CurrentFester;
        public int CurrentWilt;
        public int TargetFester;
        public int TotalWilt;
    }

    public struct FesterCalculated : IEvent
    {
        public int CalculatedFester;
        public int CurrentFester;
        public int TargetFester;
    }

    public struct FesterFinalized : IEvent
    {
        public int CalculatedFester;
        public int TargetFester;
    }

    public struct FesterFinished : IEvent { }

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
