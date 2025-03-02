using Synthesis.Calculator;
using Synthesis.Weather;

namespace Synthesis.Mutations
{
    public abstract class MutationStrategy
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        protected int trackerIndex;

        public int TrackerIndex { get => trackerIndex; set => trackerIndex = value; }

        public abstract void ApplyMutation(BattleCalculator calculator, WeatherSystem weather);
    }
}
