using Synthesis.Creatures;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;

namespace Synthesis.Turns.States
{
    public class UpgradeState : TurnState
    {
        private readonly CameraController cameraController;

        public UpgradeState(TurnSystem turnSystem, CameraController cameraController) : base(turnSystem)
        {
            this.cameraController = cameraController;
        }

        public override void OnEnter()
        {
            // Set the camera to the battle camera
            cameraController.PrioritizeBattleCamera();

            // Hide the Synthesize Shop
            EventBus<HideSynthesizeShop>.Raise(new HideSynthesizeShop());

            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "MUTATED" });

            turnSystem.AwaitEnemyTurn();
        }
    }
}
