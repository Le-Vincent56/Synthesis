namespace Synthesis.EventBus.Events.Creatures
{
    public struct PlayerAttack : IEvent { }
    public struct EnemyAttack : IEvent { }
    
    public struct PlayerHit : IEvent { }
    
    public struct EnemyHit : IEvent { }
    
    public struct EnemyDie : IEvent { }
}
