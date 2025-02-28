using Synthesis.Calculator;
using Synthesis.Weather;

namespace Synthesis.Mutations
{
    public abstract class MutationStrategy
    {
        public abstract float ApplyMutation(BattleCalculator calculator, WeatherSystem weather);
    }
}
