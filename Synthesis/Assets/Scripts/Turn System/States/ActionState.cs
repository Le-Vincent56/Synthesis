using UnityEngine;

namespace Synthesis.Turns.States
{
    public class ActionState : TurnSubState
    {
        public ActionState() { }

        public override void OnEnter()
        {
            Debug.Log("Entered Action State");
        }
    }
}
