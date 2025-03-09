using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class MonsoonBloom : MutationStrategy
    {
        public MonsoonBloom()
        {
            // Set properties
            name = "Monsoon Bloom";
            description = "If a Torrent starts mid-battle, your next Infect has +30% base Combat Rating";
            mutationType = MutationType.Active;

            partType = MutationPartType.InfectTorrent;
            color0 = new Color(0.0f, 0.65f, 0.1f,1);
            color1 = new Color(0.2f, 0.1f, 1f, 1);
            color2 = new Color(0, 1f, 1f, 1f);
        }

        /// <summary>
        /// If a Torrent starts mid-battle, your next Infect has +30% base Combat Rating
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Exit case - if the current weather is not Torrent
            if (weather.CurrentWeather is not Torrent torrent) return;

            // Exit case - if the Torrent has been active for more than 0 turns
            if (torrent.TurnsSinceStart > 0) return;

            // Increase the base Combat Rating by 30%
            calculator.IncreaseBaseAdditives(0.3f);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new MonsoonBloom();
        }
    }
}
