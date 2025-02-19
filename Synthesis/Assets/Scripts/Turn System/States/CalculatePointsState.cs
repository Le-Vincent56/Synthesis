using UnityEngine;

namespace Synthesis.Turns.States
{
    public class CalculatePointsState : TurnState
    {
        public CalculatePointsState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Calculating Points");
        }
    }
}
