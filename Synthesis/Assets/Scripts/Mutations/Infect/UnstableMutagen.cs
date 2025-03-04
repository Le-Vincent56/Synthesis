using Synthesis.Battle;
using Synthesis.Weather;
using System;
using System.Collections.Generic;

namespace Synthesis.Mutations.Infect
{
    public class UnstableMutagen : MutationStrategy
    {
        public UnstableMutagen()
        {
            // Set properties
            name = "Unstable Mutagen";
            description = "After the first Infect of the battle, randomly reroll a previous Mutation";
        }

        /// <summary>
        /// After the first Infect of the battle, randomly reroll a previous Mutation
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - the player has already infected during the battle
            if (calculator.InfectsSinceStartofBattle >= 1) return;

            // Cast this as an exception for the reroll
            List<Type> rerollExceptions = new List<Type>() { GetType() };

            // Reroll a previous Mutation
            mutations.Reroll(1, 1, rerollExceptions);
        }
    }
}
