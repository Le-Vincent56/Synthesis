using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.EventBus.Events.UI;

namespace Synthesis.Turns.States
{
    public class PlayerTurnState : TurnState
    {
        private readonly CameraController cameraController;

        private EventBinding<EnterAction> onEnterAction;
        private EventBinding<EnterMutate> onEnterMutate;

        public PlayerTurnState(TurnSystem turnSystem, CameraController cameraController) : base(turnSystem)
        {
            this.cameraController = cameraController;
        }

        ~PlayerTurnState()
        {
            EventBus<EnterAction>.Deregister(onEnterAction);
            EventBus<EnterMutate>.Deregister(onEnterMutate);
        }

        public override void OnEnter()
        {
            // Set the camera to the UI camera
            cameraController.PrioritizeUICamera();

            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "PLAYER TURN" });
            EventBus<ShowPlayerInfo>.Raise(new ShowPlayerInfo());

            // Update the turn
            turnSystem.UpdateTurns();
        }
    }
}
