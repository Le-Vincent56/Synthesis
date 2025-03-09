using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;

namespace Synthesis.Mutations.Synthesis
{
    public class RepetitiveNature : MutationStrategy
    {
        public RepetitiveNature()
        {
            name = "Repetitive Nature";
            description = "Allows Mutations to be found multiple times";
            mutationType = MutationType.Passive;

            partType = MutationPartType.Synthesis;
            color0 = new UnityEngine.Color(1.0f, 1.0f, 1.0f, 1.0f);
            color1 = new UnityEngine.Color(0.6f, 1.0f, 1.0f, 1.0f);
        }

        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Allow multiple instances of the same Mutation to be found
            mutations.AllowMultiples();
        }

        public override void OnRemove(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Disallow multiple instances of the same Mutation to be found
            mutations.DisallowMultiples();
        }

        /// <summary>
        /// Duplicate the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new RepetitiveNature();
        }
    }
}
