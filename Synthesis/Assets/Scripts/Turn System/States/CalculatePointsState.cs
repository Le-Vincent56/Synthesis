using Synthesis.Creatures;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using System.Text;
using Synthesis.Battle;

namespace Synthesis.Turns.States
{
    public class CalculatePointsState : TurnState
    {
        private readonly BattleCalculator battleCalculator;
        private readonly CameraController cameraController;

        public CalculatePointsState(TurnSystem turnSystem, BattleCalculator battleCalculator, CameraController cameraController) : base(turnSystem)
        {
            this.battleCalculator = battleCalculator;
            this.cameraController = cameraController;
        }

        public override void OnEnter()
        {
            // Set the camera to the battle camera
            cameraController.PrioritizeBattleCamera();

            // Hide the player info
            EventBus<HidePlayerInfo>.Raise(new HidePlayerInfo()); 

            // Build the info text
            StringBuilder sb = new StringBuilder();
            sb.Append("Dealt ");
            sb.Append(battleCalculator.CalculatePoints());
            sb.Append(" damage");

            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader{ Text = sb.ToString() });

            turnSystem.AwaitEnemyTurn();
        }
    }
}
