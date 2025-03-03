using Synthesis.Creatures;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using System.Text;
using Synthesis.Battle;
using Synthesis.EventBus.Events.Battle;

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

            // Calculate combat rating
            int combatRating = battleCalculator.CalculatePoints();

            // Build the info text
            StringBuilder sb = new StringBuilder();
            sb.Append("Dealt ");
            sb.Append(combatRating);
            sb.Append(" damage");

            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader{ Text = sb.ToString() });

            // State that the combat rating has been calculated
            EventBus<CombatRatingCalculated>.Raise(new CombatRatingCalculated { CombatRating = combatRating });

            turnSystem.AwaitEnemyTurn();
        }
    }
}
