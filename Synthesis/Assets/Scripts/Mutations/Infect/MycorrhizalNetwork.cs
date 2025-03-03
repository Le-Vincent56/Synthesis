using Synthesis.Calculator;
using Synthesis.Creatures;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class MycorrhizalNetwork : MutationStrategy
    {
        public MycorrhizalNetwork()
        {
            name = "Mycorrhizal Network";
            description = "Infect gains +5% higher base Combat Rating per consecutive use";
            partType = MutationPartType.Infect;
            color0 = new Color(0.7f, 0.55f, 0.8f,1);
            color1 = new Color(0.86f, 0.76f, 0.2f, 1);
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
