using Synthesis.Creatures;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using UnityEngine;
using System.Text;

namespace Synthesis.Turns.States
{
    public class CalculatePointsState : TurnState
    {
        private readonly Player player;

        public CalculatePointsState(TurnSystem turnSystem, Player player) : base(turnSystem)
        {
            this.player = player;
        }

        public override void OnEnter()
        {
            // Build the info text
            StringBuilder sb = new StringBuilder();
            sb.Append("Dealt ");
            sb.Append(player.CalculatePoints());
            sb.Append(" damage");

            // Set the text
            EventBus<SetInfoText>.Raise(new SetInfoText { Text = sb.ToString() });

            turnSystem.AwaitEnemyTurn();
        }
    }
}
