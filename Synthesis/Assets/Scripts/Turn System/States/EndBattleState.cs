using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class EndBattleState : TurnState
    {
        public EndBattleState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            EventBus<SetInfoText>.Raise(new SetInfoText { Text = "Battle Ended" });

            // Start the next battle
            turnSystem.NextBattle();
        }
    }
}
