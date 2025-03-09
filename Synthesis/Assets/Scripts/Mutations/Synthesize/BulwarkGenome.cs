using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Weather;

namespace Synthesis.Mutations.Synthesis
{
    public class BulwarkGenome : MutationStrategy
    {
        private int mutationsSinceReceived;

        public BulwarkGenome()
        {
            // Set properties
            name = "Bulwark Genome";
            description = "Gain an extra turn every 4 Mutations";
            mutationType = MutationType.Passive;
            mutationsSinceReceived = 0;

            partType = MutationPartType.Synthesis;
            color0 = new UnityEngine.Color(0.3f, 1.0f, 0.3f, 1.0f);
            color1 = new UnityEngine.Color(0.3f, 1.0f, 0.3f, 1.0f);
        }

        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Increase the number of mutations since the last extra turn
            mutationsSinceReceived++;

            // If the number of mutations since the last extra turn is 4, give an extra turn
            if (mutationsSinceReceived % 4 != 0) return;

            // Get an extra turn
            EventBus<GainTurns>.Raise(new GainTurns() { TurnsToGain = 1 });
        }

        public override MutationStrategy Duplicate()
        {
            return new BulwarkGenome();
        }
    }
}
