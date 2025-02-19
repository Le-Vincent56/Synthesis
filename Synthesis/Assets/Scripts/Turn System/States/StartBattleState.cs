using UnityEngine;

namespace Synthesis.Turns.States
{
    public class StartBattleState : TurnState
    {
        public StartBattleState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Started Battle");
        }
    }
}
