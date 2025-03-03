using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.EventBus.Events.UI;

namespace Synthesis.Turns.States
{
    public class StartBattleState : TurnState
    {
        public StartBattleState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            EventBus<StartBattle>.Raise(new StartBattle());

            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader{ Text = "ENCOUNTER START" });

            turnSystem.AwaitPlayerTurn();
        }
    }
}
