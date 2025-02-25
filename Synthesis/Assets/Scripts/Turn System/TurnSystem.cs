using Synthesis.Creatures;
using Synthesis.Timers;
using Synthesis.Turns.States;
using Synthesis.Utilities.StateMachine;
using UnityEngine;

namespace Synthesis.Turns
{
    public class TurnSystem : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private Player player;
        [SerializeField] private int state;
        private StateMachine stateMachine;
        private int turnsRemaining;

        private CountdownTimer startBattleTimer;
        private CountdownTimer setPlayerTurnTimer;
        private CountdownTimer setEnemyTurnTimer;
        private CountdownTimer setEnemyDamageTimer;
        private CountdownTimer endTurnTimer;

        private void Awake()
        {
            // Set initial state
            state = 0;

            // Create the Start Battle Timer
            CreateTimers();

            // Set up the State Machine
            SetupStateMachine();

            turnsRemaining = 4;
        }

        private void Update()
        {
            // Update the State Machine
            stateMachine?.Update();
        }

        private void OnDestroy()
        {
            // Dispose of Timers
            startBattleTimer?.Dispose();
            setPlayerTurnTimer?.Dispose();
            setEnemyTurnTimer?.Dispose();
            setEnemyDamageTimer?.Dispose();
            endTurnTimer?.Dispose();
        }

        /// <summary>
        /// Set up the State Machine
        /// </summary>
        private void SetupStateMachine()
        {
            // Initialize the State Machine
            stateMachine = new StateMachine();

            // Create states
            StartBattleState startBattle = new StartBattleState(this);
            PlayerTurnState playerTurn = new PlayerTurnState(this);
            CalculatePointsState calculatePoints = new CalculatePointsState(this, player);
            EnemyTurnState enemyTurn = new EnemyTurnState(this);
            CalculateDamageState calculateDamage = new CalculateDamageState(this);
            EndBattleState endBattle = new EndBattleState(this);

            // Define state transitions
            stateMachine.At(startBattle, playerTurn, new FuncPredicate(() => state == 1));
            stateMachine.At(playerTurn, calculatePoints, new FuncPredicate(() => state == 2));
            stateMachine.At(calculatePoints, enemyTurn, new FuncPredicate(() => state == 3));
            stateMachine.At(enemyTurn, calculateDamage, new FuncPredicate(() => state == 4));
            stateMachine.At(calculateDamage, endBattle, new FuncPredicate(() => state == 5));
            stateMachine.At(calculateDamage, playerTurn, new FuncPredicate(() => state == 1));
            stateMachine.At(endBattle, startBattle, new FuncPredicate(() => state == 0));

            // Set initial state
            stateMachine.SetState(startBattle);
        }

        /// <summary>
        /// Set the amount of turns
        /// </summary>
        public void SetTurns(int turns) => turnsRemaining = turns;

        /// <summary>
        /// Pass the turn
        /// </summary>
        public void PassTurn()
        {
            // Check if there are turns remaining
            if (turnsRemaining > 0)
            {
                // Decrement the turns remaining and set the player state
                turnsRemaining--;
                state = 1;

                return;
            }

            // Otherwise, set the end state
            state = 5;
            turnsRemaining = 4;
        }

        /// <summary>
        /// Create the Start Battle Timer
        /// </summary>
        private void CreateTimers()
        {
            startBattleTimer = new CountdownTimer(1f);
            startBattleTimer.OnTimerStop += () => state = 0;

            setPlayerTurnTimer = new CountdownTimer(1f);
            setPlayerTurnTimer.OnTimerStop += () => state = 1;

            setEnemyTurnTimer = new CountdownTimer(1f);
            setEnemyTurnTimer.OnTimerStop += () => state = 3;

            setEnemyDamageTimer = new CountdownTimer(1f);
            setEnemyDamageTimer.OnTimerStop += () => state = 4;

            endTurnTimer = new CountdownTimer(1f);
            endTurnTimer.OnTimerStop += () => PassTurn();
        }

        /// <summary>
        /// Set the State of the Turn System
        /// </summary>
        public void SetState(int state) => this.state = state;

        /// <summary>
        /// Start the next battle
        /// </summary>
        public void NextBattle() => startBattleTimer.Start();

        /// <summary>
        /// Await the player turn phase
        /// </summary>
        public void AwaitPlayerTurn() => setPlayerTurnTimer?.Start();

        public void AwaitEnemyTurn() => setEnemyTurnTimer?.Start();

        public void AwaitEnemyDamage() => setEnemyDamageTimer?.Start();

        public void AwaitPassTurn() => endTurnTimer?.Start();
    }
}
