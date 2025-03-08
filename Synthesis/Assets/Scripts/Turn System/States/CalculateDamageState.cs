using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;
using Synthesis.Timers;
using Synthesis.EventBus.Events.Creatures;

namespace Synthesis.Turns.States
{
    public class CalculateDamageState : TurnState
    {
        private readonly CameraController cameraController;
        private readonly SpawnCreaturesEvil spawnCreaturesEvil;
        private CountdownTimer attackTimer;
        private CountdownTimer waitForAttackTimer;

        public CalculateDamageState(TurnSystem turnSystem, CameraController cameraController, SpawnCreaturesEvil spawnCreaturesEvil) : base(turnSystem)
        {
            this.cameraController = cameraController;
            this.spawnCreaturesEvil = spawnCreaturesEvil;

            attackTimer = new CountdownTimer(1.0f);
            attackTimer.OnTimerStop += () =>
            {
                // Generate impulse and have enemies attack
                EventBus<EnemyAttack>.Raise(new EnemyAttack());

                // STart the next timer
                waitForAttackTimer.Start();
            };

            waitForAttackTimer = new CountdownTimer(0.75f);
            waitForAttackTimer.OnTimerStop += () =>
            {
                EventBus<PlayerHit>.Raise(new PlayerHit());
                
                // Apply wilt
                int wiltToApply = 5 * this.spawnCreaturesEvil.EvilCreaturesCount;

                EventBus<ApplyWilt>.Raise(new ApplyWilt() { WiltToApply = wiltToApply });

                // Pass the turn
                this.turnSystem.AwaitPassTurn();
            };
        }

        ~CalculateDamageState()
        {
            // Dispose of tiemrs
            attackTimer?.Dispose();
            waitForAttackTimer?.Dispose();
        }

        public override void OnEnter()
        {
            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "ENEMY DAMAGE" });

            // Start the attack timer
            attackTimer.Start();
        }
    }
}
