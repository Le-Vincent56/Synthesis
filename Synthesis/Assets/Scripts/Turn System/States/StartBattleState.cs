using Synthesis.EventBus;
using Synthesis.EventBus.Events.UI;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class StartBattleState : TurnState
    {
        public StartBattleState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            EventBus<SetInfoText>.Raise(new SetInfoText { Text = "Battle Started" });

            turnSystem.AwaitPlayerTurn();
        }
    }
}
