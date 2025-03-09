using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;
using Synthesis.EventBus.Events.Turns;
using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus.Events.Weather;
using Synthesis.ServiceLocators;
using Synthesis.Timers;
using Synthesis.Turns.States;
using Synthesis.Utilities.StateMachine;
using UnityEngine;

namespace Synthesis.Turns
{
    public class TurnSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BattleCalculator battleCalculator;
        [SerializeField] private BattleMetrics metrics;
        [SerializeField] private SpawnCreaturesEvil spawnCreaturesEvil;

        [Header("States")]
        [SerializeField] private Player player;
        [SerializeField] private int state;
        private StateMachine stateMachine;
        private int currentTurn;
        private int totalTurns;
        private int currentRound;

        private CountdownTimer startBattleTimer;
        private CountdownTimer setPlayerTurnTimer;
        private CountdownTimer updateTurnTimer;
        private CountdownTimer setEnemyTurnTimer;
        private CountdownTimer setEnemyDamageTimer;
        private CountdownTimer endTurnTimer;

        private EventBinding<Infect> onInfect;
        private EventBinding<Synthesize> onSynthesize;
        private EventBinding<FesterFinished> onCombatRatingCalculationFinished;
        private EventBinding<WinBattle> onEndBattle;
        private EventBinding<Wilted> onWilted;
        public int CurrentRound { get => currentRound; }

        private void Awake()
        {
            // Set initial state
            state = 0;

            // Set the current round to 0
            currentRound = 0;

            // Create the Start Battle Timer
            CreateTimers();

            currentTurn = 1;
            totalTurns = 7;
        }

        private void OnEnable()
        {
            onInfect = new EventBinding<Infect>(Infect);
            EventBus<Infect>.Register(onInfect);

            onSynthesize = new EventBinding<Synthesize>(Synthesize);
            EventBus<Synthesize>.Register(onSynthesize);

            onCombatRatingCalculationFinished = new EventBinding<FesterFinished>(GoToEnemyTurn);
            EventBus<FesterFinished>.Register(onCombatRatingCalculationFinished);

            onEndBattle = new EventBinding<WinBattle>(WinBattle);
            EventBus<WinBattle>.Register(onEndBattle);

            onWilted = new EventBinding<Wilted>(LoseTurn);
            EventBus<Wilted>.Register(onWilted);
        }

        private void OnDisable()
        {
            EventBus<Infect>.Deregister(onInfect);
            EventBus<Synthesize>.Deregister(onSynthesize);
            EventBus<FesterFinished>.Deregister(onCombatRatingCalculationFinished);
            EventBus<WinBattle>.Deregister(onEndBattle);
            EventBus<Wilted>.Deregister(onWilted);
        }

        private void Start()
        {
            // Retrieve services
            cameraController = ServiceLocator.ForSceneOf(this).Get<CameraController>();
            battleCalculator = ServiceLocator.ForSceneOf(this).Get<BattleCalculator>();
            metrics = ServiceLocator.ForSceneOf(this).Get<BattleMetrics>();
            spawnCreaturesEvil = ServiceLocator.ForSceneOf(this).Get<SpawnCreaturesEvil>();

            // Set up the State Machine
            SetupStateMachine();
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
            updateTurnTimer?.Dispose();
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
            PlayerTurnState playerTurn = new PlayerTurnState(this, cameraController);
            MutateState mutateState = new MutateState(this, cameraController);
            CalculatePointsState calculatePoints = new CalculatePointsState(this, battleCalculator, metrics, cameraController);
            EnemyTurnState enemyTurn = new EnemyTurnState(this);
            CalculateDamageState calculateDamage = new CalculateDamageState(this, cameraController, spawnCreaturesEvil);
            EndBattleState endBattle = new EndBattleState(this);

            // Define state transitions
            stateMachine.At(startBattle, playerTurn, new FuncPredicate(() => state == 1));

            stateMachine.At(playerTurn, calculatePoints, new FuncPredicate(() => state == 2));
            stateMachine.At(playerTurn, mutateState, new FuncPredicate(() => state == 3));

            stateMachine.At(mutateState, enemyTurn, new FuncPredicate(() => state == 4));

            stateMachine.At(calculatePoints, enemyTurn, new FuncPredicate(() => state == 4));
            stateMachine.At(calculatePoints, endBattle, new FuncPredicate(() => state == 6));

            stateMachine.At(enemyTurn, calculateDamage, new FuncPredicate(() => state == 5));

            stateMachine.At(calculateDamage, endBattle, new FuncPredicate(() => state == 6));
            stateMachine.At(calculateDamage, playerTurn, new FuncPredicate(() => state == 1));

            stateMachine.At(endBattle, startBattle, new FuncPredicate(() => state == 0));

            // Set initial state
            stateMachine.SetState(startBattle);
        }

        /// <summary>
        /// Set the amount of turns
        /// </summary>
        public void SetTurns(int turns) => totalTurns = turns;

        /// <summary>
        /// Pass the turn
        /// </summary>
        public void PassTurn()
        {
            // Check if there are turns remaining
            if (currentTurn < totalTurns)
            {
                // Decrement the turns remaining and set the player state
                currentTurn++;
                state = 1;

                return;
            }

            // Lose the battle
            EventBus<LoseBattle>.Raise(new LoseBattle());
        }

        /// <summary>
        /// Lose a turn
        /// </summary>
        private void LoseTurn()
        {
            // Lose a turn
            totalTurns--;

            // Check if the current turn is less than the total turns
            if (currentTurn <= totalTurns) return;

            // Lose the battle immediately
            EventBus<LoseBattle>.Raise(new LoseBattle());

            // Update the amount of turns in the UI
            EventBus<UpdateTurns>.Raise(new UpdateTurns() { CurrentTurn = currentTurn, TotalTurns = totalTurns });
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

            updateTurnTimer = new CountdownTimer(0.75f);
            updateTurnTimer.OnTimerStop += () => EventBus<UpdateTurns>.Raise(new UpdateTurns { CurrentTurn = currentTurn, TotalTurns = totalTurns });

            setEnemyTurnTimer = new CountdownTimer(3f);
            setEnemyTurnTimer.OnTimerStop += () => state = 4;

            setEnemyDamageTimer = new CountdownTimer(3f);
            setEnemyDamageTimer.OnTimerStop += () => state = 5;

            endTurnTimer = new CountdownTimer(3f);
            endTurnTimer.OnTimerStop += () => PassTurn();
        }

        /// <summary>
        /// Take the Infect action by changing turn states
        /// </summary>
        private void Infect() => SetState(2);

        /// <summary>
        /// Take the Synthesize action by changing turn states
        /// </summary>
        private void Synthesize() => SetState(3);

        private void GoToEnemyTurn(FesterFinished eventData) => state = 4;

        /// <summary>
        /// Set the State of the Turn System
        /// </summary>
        public void SetState(int state) => this.state = state;

        /// <summary>
        /// Start the next battle
        /// </summary>
        public void NextBattle() => startBattleTimer.Start();

        /// <summary>
        /// Update the turns
        /// </summary>
        public void UpdateTurns() => updateTurnTimer?.Start();

        /// <summary>
        /// Await the player turn phase
        /// </summary>
        public void AwaitPlayerTurn() => setPlayerTurnTimer?.Start();

        /// <summary>
        /// Await the enemy turn phase
        /// </summary>
        public void AwaitEnemyTurn() => setEnemyTurnTimer?.Start();

        /// <summary>
        /// Await the enemy damage phase
        /// </summary>
        public void AwaitEnemyDamage() => setEnemyDamageTimer?.Start();

        /// <summary>
        /// Await the end turn phase
        /// </summary>
        public void AwaitPassTurn() => endTurnTimer?.Start();

        private void WinBattle()
        {
            // Set the state to 6
            state = 6;
            totalTurns = 7;
            currentTurn = 1;
            currentRound++;
        }
    }
}
