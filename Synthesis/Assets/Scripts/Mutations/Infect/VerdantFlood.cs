using Synthesis.Battle;
using Synthesis.Weather;
using System;
using Synthesis.Creatures;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class VerdantFlood : MutationStrategy
    {
        public VerdantFlood()
        {
            // Set properties
            name = "Verdant Flood";
            description = "In Torrent, every third Infect grants +5 base Combat Rating permanently";
            mutationType = MutationType.Active;

            partType = MutationPartType.InfectTorrent;
            color0 = new Color(0.0f, 0.65f, 0.1f,1);
            color1 = new Color(0.7f, 0.55f, 0.8f,1);
            color2 = new Color(0, 1f, 1f, 1f);
        }

        /// <summary>
        /// In Torrent, every third Infect grants +5 base Combat Rating permanently
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - if the current weather is not Torrent
            if (weather.CurrentWeather is not Torrent torrent) return;

            // Exit case - if there are infects since the start of the Drought
            if (torrent.NumberOfInfectsSinceStart != 0 && torrent.NumberOfInfectsSinceStart % 3 == 0) return;

            // Increase the base Combat Rating permanently by 5
            calculator.IncreaseBasePermenentAdditive(5f);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new VerdantFlood();
        }
    }
}
