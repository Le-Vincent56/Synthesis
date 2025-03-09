using Synthesis.Battle;
using Synthesis.Weather;
using System;
using System.Collections.Generic;
using Synthesis.Creatures;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class UnstableMutagen : MutationStrategy
    {
        public UnstableMutagen()
        {
            // Set properties
            name = "Unstable Mutagen";
            description = "After the first Infect of the battle, randomly reroll a previous Mutation";
            partType = MutationPartType.Infect;
            color1 = new Color(1.0f, 1.0f, 1.0f,1);
            color0 = new Color(0.1f, 0.2f, 0.1f, 1);
            mutationType = MutationType.Active;
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

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new UnstableMutagen();
        }
    }
}
