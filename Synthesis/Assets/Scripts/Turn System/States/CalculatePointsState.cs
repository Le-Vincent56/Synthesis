using Synthesis.Creatures;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.Battle;
using Synthesis.EventBus.Events.Battle;
using Synthesis.Timers;
using Cinemachine;
using Synthesis.EventBus.Events.Creatures;

namespace Synthesis.Turns.States
{
    public class CalculatePointsState : TurnState
    {
        private readonly BattleCalculator battleCalculator;
        private readonly BattleMetrics metrics;
        private readonly CameraController cameraController;
        private CountdownTimer attackTimer;
        private CountdownTimer calculatePointsTimer;

        public CalculatePointsState(TurnSystem turnSystem, BattleCalculator battleCalculator, BattleMetrics metrics, CameraController cameraController) : base(turnSystem)
        {
            this.battleCalculator = battleCalculator;
            this.cameraController = cameraController;
            this.metrics = metrics;

            attackTimer = new CountdownTimer(1.5f);
            attackTimer.OnTimerStop += () =>
            {
                // Generate impulse and have enemies attack
                EventBus<PlayerAttack>.Raise(new PlayerAttack());

                this.cameraController.GenerateImpulse();

                // Start the next timer
                calculatePointsTimer.Start();
            };

            calculatePointsTimer = new CountdownTimer(0.75f);
            calculatePointsTimer.OnTimerStop += () =>
            {
                // Calculate combat rating
                int combatRatingCalculated = this.battleCalculator.CalculatePoints();
                int combatRatingFull = metrics.CurrentCombatRating + combatRatingCalculated;

                // State that the combat rating has been calculated
                EventBus<CombatRatingCalculated>.Raise(new CombatRatingCalculated { CombatRatingPointsCalculated = combatRatingCalculated, CombatRatingCurrent = combatRatingFull });
            };
        }

        ~CalculatePointsState()
        {
            // Dispose of the timer
            calculatePointsTimer?.Dispose();
        }

        public override void OnEnter()
        {
            // Hide the player info
            EventBus<HidePlayerInfo>.Raise(new HidePlayerInfo());

            EventBus<HideTurnHeader>.Raise(new HideTurnHeader());

            // Set the camera to the battle camera
            cameraController.PrioritizeBattleCamera();

            // Start the timer
            attackTimer.Start();
        }
    }
}
