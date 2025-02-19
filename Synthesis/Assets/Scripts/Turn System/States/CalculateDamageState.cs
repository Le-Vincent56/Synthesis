using UnityEngine;

namespace Synthesis.Turns.States
{
    public class CalculateDamageState : TurnState
    {
        public CalculateDamageState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Calculating Damage State");
        }
    }
}
