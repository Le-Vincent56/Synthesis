using UnityEngine;

namespace Synthesis.Turns.States
{
    public class SynthesizeState : TurnState
    {
        public SynthesizeState(TurnSystem turnSystem) : base(turnSystem) { } 

        public override void OnEnter()
        {
            Debug.Log("Entered Mutate State");
        }
    }
}
