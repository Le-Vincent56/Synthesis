using UnityEngine;

namespace Synthesis.Turns.States
{
    public class EnemyTurnState : TurnState
    {
        public EnemyTurnState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Entered Enemy Turn");
        }
    }
}
