namespace Synthesis.EventBus.Events.Turns
{
    public struct StartBattle : IEvent { }

    public struct StartPlayerTurn : IEvent { }
    public struct EnterAction : IEvent { }
    public struct EnterMutate : IEvent { }

    public struct EndPlayerTurn : IEvent { }

    public struct CalculateCombatPoints : IEvent { }

    public struct StartEnemyTurn : IEvent { }

    public struct EndEnemyTurn : IEvent { }

    public struct CalculateDamageThreshold : IEvent { }

    public struct EndBattle : IEvent
    {
        public bool Continue;
    }
}
