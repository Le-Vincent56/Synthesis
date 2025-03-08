using Synthesis.EventBus.Events.UI;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Creatures;
using Synthesis.Timers;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class EndBattleState : TurnState
    {
        private CountdownTimer deathTimer;
        
        public EndBattleState(TurnSystem turnSystem) : base(turnSystem)
        {
            deathTimer = new CountdownTimer(2.0f);
            deathTimer.OnTimerStop += () =>
            {
                // Start the next battle
                turnSystem.NextBattle();
            };
        }
        
        ~EndBattleState()
        {
            deathTimer?.Dispose();
        }

        public override void OnEnter()
        {
            EventBus<SetInfoText>.Raise(new SetInfoText { Text = "Battle Ended" });
            
            EventBus<EnemyDie>.Raise(new EnemyDie());
            
            deathTimer.Start();
        }
    }
}
