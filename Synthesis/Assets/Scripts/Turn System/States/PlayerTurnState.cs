using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Utilities.StateMachine;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class PlayerTurnState : TurnState
    {
        private StateMachine subStateMachine;
        private ActionState action;
        private MutateState mutate;

        private int state;

        private EventBinding<EnterAction> onEnterAction;
        private EventBinding<EnterMutate> onEnterMutate;

        public PlayerTurnState(TurnSystem turnSystem) : base(turnSystem)
        {
            // Set the initial state
            state = 0;

            // Set up the sub-State Machine
            SetupStateMachine();

            onEnterAction = new EventBinding<EnterAction>(EnterActionState);
            EventBus<EnterAction>.Register(onEnterAction);

            onEnterMutate = new EventBinding<EnterMutate>(EnterMutateState);
            EventBus<EnterMutate>.Register(onEnterMutate);
        }

        ~PlayerTurnState()
        {
            EventBus<EnterAction>.Deregister(onEnterAction);
            EventBus<EnterMutate>.Deregister(onEnterMutate);
        }
        
        /// <summary>
        /// Set up the sub-State Machine
        /// </summary>
        private void SetupStateMachine()
        {
            // Initialize the State Machine
            subStateMachine = new StateMachine();

            // Create sub-States
            action = new ActionState();
            mutate = new MutateState();

            // Define sub-State transitions
            subStateMachine.At(action, mutate, new FuncPredicate(() => state == 1));
            subStateMachine.At(mutate, action, new FuncPredicate(() => state == 0));

            // Set the initial state
            subStateMachine.SetState(action);
        }

        public override void OnEnter()
        {
            // Set the action state
            state = 0;
            subStateMachine.SetState(action);

            Debug.Log("Entered Player Turn");
        }

        public override void Update()
        {
            // Update the sub-State Machine
            subStateMachine?.Update();
        }

        private void EnterActionState() => state = 0;
        private void EnterMutateState() => state = 1;
    }
}
