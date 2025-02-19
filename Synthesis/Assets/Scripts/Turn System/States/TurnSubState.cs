using Synthesis.Utilities.StateMachine;

namespace Synthesis.Turns.States
{
    public class TurnSubState : IState
    {
        public TurnSubState() 
        {

        }

        public virtual void OnEnter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnExit() { }
    }
}
