using Synthesis.Calculator;
using Synthesis.Weather;

namespace Synthesis.Mutations
{
    public abstract class MutationStrategy
    {
        protected int trackerIndex;

        public int TrackerIndex { get => trackerIndex; set => trackerIndex = value; }

        public abstract void ApplyMutation(BattleCalculator calculator, WeatherSystem weather);
    }
}
