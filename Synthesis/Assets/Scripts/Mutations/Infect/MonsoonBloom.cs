using Synthesis.Calculator;
using Synthesis.Weather;

namespace Synthesis.Mutations.Infect
{
    public class MonsoonBloom : MutationStrategy
    {
        public MonsoonBloom()
        {
            name = "Monsoon Bloom";
            description = "If a Torrent starts mid-battle, your next Infect has +30% base Combat Rating";
        }

        /// <summary>
        /// If a Torrent starts mid-battle, your next Infect has +30% base Combat Rating
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather)
        {
            // Exit case - if the current weather is not Torrent
            if (weather.CurrentWeather is not Torrent torrent) return;

            // Exit case - if the Torrent has been active for more than 0 turns
            if (torrent.TurnsSinceStart > 0) return;

            // Increase the base Combat Rating by 30%
            calculator.IncreaseBaseAdditives(0.3f);
        }
    }
}
