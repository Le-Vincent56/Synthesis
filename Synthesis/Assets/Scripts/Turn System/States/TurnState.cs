using Synthesis.Utilities.StateMachine;

namespace Synthesis.Turns.States
{
    public class TurnState : IState
    {
        protected readonly TurnSystem turnSystem;

        public TurnState(TurnSystem turnSystem)
        {
            this.turnSystem = turnSystem;
        }

        public virtual void OnEnter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnExit() { }
    }
}
