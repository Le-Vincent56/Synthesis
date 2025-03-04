using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;

namespace Synthesis.Turns.States
{
    public class CalculateDamageState : TurnState
    {
        public CalculateDamageState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            EventBus<ApplyWilt>.Raise(new ApplyWilt() { WiltToApply = 10 });

            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "ENEMY DAMAGE" });

            // Pass the turn
            turnSystem.AwaitPassTurn();
        }
    }
}
