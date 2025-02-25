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
            EventBus<SetInfoText>.Raise(new SetInfoText { Text = "Calculating Enemy Damage..." });

            // Pass the turn
            turnSystem.AwaitPassTurn();
        }
    }
}
