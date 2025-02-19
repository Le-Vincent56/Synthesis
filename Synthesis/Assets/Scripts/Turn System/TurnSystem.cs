using Synthesis.EventBus.Events.Turns;
using Synthesis.Timers;
using Synthesis.Turns.States;
using Synthesis.Utilities.StateMachine;
using UnityEngine;

namespace Synthesis.Turns
{
    public class TurnSystem : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private int state;
        private StateMachine stateMachine;

        private CountdownTimer startBattleTimer;

        private void Awake()
        {
            // Set initial state
            state = 0;

            // Set up the State Machine
            SetupStateMachine();

            // Create the Start Battle Timer
            CreateStartTimer();
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
            CalculatePointsState calculatePoints = new CalculatePointsState(this);
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
        /// Create the Start Battle Timer
        /// </summary>
        private void CreateStartTimer()
        {
            startBattleTimer = new CountdownTimer(1f);
            startBattleTimer.OnTimerStop += () => state = 0;
        }

        /// <summary>
        /// Set the State of the Turn System
        /// </summary>
        public void SetState(int state) => this.state = state;

        /// <summary>
        /// Start the next battle
        /// </summary>
        public void NextBattle() => startBattleTimer.Start();
    }
}
