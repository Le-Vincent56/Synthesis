using Synthesis.Calculator;
using Synthesis.Weather;

namespace Synthesis.Mutations.Infect
{
    public class MycorrhizalNetwork : MutationStrategy
    {
        public MycorrhizalNetwork()
        {
            name = "Mycorrhizal Network";
            description = "Infect gains +5% higher base Combat Rating per consecutive use";
        }

        /// <summary>
        /// Infect gains +5% higher base Combat Rating per consecutive use
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather)
        {
            calculator.IncreaseBaseAdditives(0.05f * calculator.ConsecutiveInfects);
        }
    }
}
