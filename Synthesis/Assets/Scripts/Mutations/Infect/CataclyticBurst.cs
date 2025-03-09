using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class CataclyticBurst : MutationStrategy
    {
        private int infectCounter = 0;

        public CataclyticBurst()
        {
            name = "Cataclytic Burst";
            description = "For every three Infect actions, the next one increases its base Combat Rating by +100%";
            partType = MutationPartType.Infect;
            color0 = new Color(0.4f, 0.3f, 1.0f,1);
            color1 = new Color(0.0f, 1.0f, 0.0f, 1);
            mutationType = MutationType.Active;
        }

        /// <summary>
        /// For every three Infect actions, the next one increases its base Combat Rating by +100%
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Increase the current infect counter
            infectCounter++;

            // If the infect counter is divisible by 3, increase the base Combat Rating by 100%
            if (infectCounter % 3 != 0) return;

            // Increase the base Combat Rating by 100%
            calculator.IncreaseBaseAdditives(1.0f);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new CataclyticBurst();
        }
    }
}
