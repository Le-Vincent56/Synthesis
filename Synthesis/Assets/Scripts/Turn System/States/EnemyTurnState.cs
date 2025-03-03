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
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "ENEMY TURN" });

            turnSystem.AwaitEnemyDamage();
        }
    }
}
