using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;

namespace Synthesis.Turns.States
{
    public class CalculateDamageState : TurnState
    {
        private readonly SpawnCreaturesEvil spawnCreaturesEvil;

        public CalculateDamageState(TurnSystem turnSystem, SpawnCreaturesEvil spawnCreaturesEvil) : base(turnSystem)
        {
            this.spawnCreaturesEvil = spawnCreaturesEvil;
        }

        public override void OnEnter()
        {
            int wiltToApply = 5 * spawnCreaturesEvil.EvilCreaturesCount;

            EventBus<ApplyWilt>.Raise(new ApplyWilt() { WiltToApply = wiltToApply });

            // Set the text
            EventBus<ShowTurnHeader>.Raise(new ShowTurnHeader { Text = "ENEMY DAMAGE" });

            // Pass the turn
            turnSystem.AwaitPassTurn();
        }
    }
}
