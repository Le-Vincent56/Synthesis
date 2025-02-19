using UnityEngine;

namespace Synthesis.Turns.States
{
    public class EndBattleState : TurnState
    {
        public EndBattleState(TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Ended Battle");

            // Start the next battle
            turnSystem.NextBattle();
        }
    }
}
