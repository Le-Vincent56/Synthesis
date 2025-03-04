using Synthesis.Mutations;

namespace Synthesis.EventBus.Events.Turns
{
    public struct StartBattle : IEvent { }

    public struct StartPlayerTurn : IEvent { }
    public struct EnterAction : IEvent { }
    public struct EnterMutate : IEvent { }
    public struct Infect : IEvent { }
    public struct Synthesize : IEvent
    {
        public MutationStrategy Mutation;
    }

    public struct CalculateCombatPoints : IEvent { }

    public struct StartEnemyTurn : IEvent { }

    public struct EndEnemyTurn : IEvent { }

    public struct CalculateDamageThreshold : IEvent { }

    public struct WinBattle : IEvent { }

    public struct LoseBattle : IEvent { }
}
