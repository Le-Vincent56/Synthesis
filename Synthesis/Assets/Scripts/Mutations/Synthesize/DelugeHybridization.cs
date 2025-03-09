using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;

namespace Synthesis.Mutations.Synthesis
{
    public class DelugeHybridization : MutationStrategy
    {
        public DelugeHybridization()
        {
            name = "Deluge Hybridization";
            description = "When you choose a Mutation in Torrent, you have a 10% chance to duplicate a random Mutation";

            mutationType = MutationType.Passive;
            partType = MutationPartType.SynthesisTorrent;
            color0 = new UnityEngine.Color(0.0f, 1.0f, 0.6f, 1.0f);
            color1 = new UnityEngine.Color(0.3f, 0.0f, 1.0f, 1.0f);
            color2 = new UnityEngine.Color(0.0f, 0.0f, 1.0f, 1.0f);
        }

        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - if the current weather is not Torrent
            if (weather.CurrentWeather is not Torrent torrent) return;

            // Calculate the chance to shift
            float chanceToDuplicate = UnityEngine.Random.Range(0.0f, 1.0f);

            // Exit case - the random value is outside the range
            if (chanceToDuplicate > 0.1f) return;

            // Duplicate a random Mutation
            mutations.DuplicateRandom();
        }

        /// <summary>
        /// Duplicate the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new DelugeHybridization();
        }
    }
}
