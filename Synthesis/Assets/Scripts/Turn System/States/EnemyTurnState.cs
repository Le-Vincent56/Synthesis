using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class EnemyTurnState : TurnState
    {
        public EnemyTurnState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            EventBus<SetInfoText>.Raise(new SetInfoText { Text = "Enemy Turn" });

            turnSystem.AwaitEnemyDamage();
        }
    }
}
