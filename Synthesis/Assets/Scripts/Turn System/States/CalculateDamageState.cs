using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;

namespace Synthesis.Turns.States
{
    public class CalculateDamageState : TurnState
    {
        public CalculateDamageState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "ENEMY DAMAGE" });

            // Pass the turn
            turnSystem.AwaitPassTurn();
        }
    }
}
