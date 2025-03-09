using Synthesis.Battle;
using Synthesis.Creatures;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Weather;
using UnityEngine;

namespace Synthesis.Mutations.Infect
{
    public class MycorrhizalNetwork : MutationStrategy
    {
        [SerializeField] private int consecutiveInfects = 0;

        private EventBinding<EventBus.Events.Turns.Infect> onInfect;
        private EventBinding<Synthesize> onSynthesize;

        public MycorrhizalNetwork()
        {
            // Set properties
            name = "Mycorrhizal Network";
            description = "Infect gains +5% higher base Combat Rating per consecutive use";
            mutationType = MutationType.Active;

            partType = MutationPartType.Infect;
            color0 = new Color(0.7f, 0.55f, 0.8f,1);
            color1 = new Color(0.86f, 0.76f, 0.2f, 1);

            consecutiveInfects = 0;

            // Register to events
            onInfect = new EventBinding<EventBus.Events.Turns.Infect>(IncreaseConsecutiveInfects);
            EventBus<EventBus.Events.Turns.Infect>.Register(onInfect);

            onSynthesize = new EventBinding<Synthesize>(ResetConsecutiveInfects);
            EventBus<Synthesize>.Register(onSynthesize);
        }

        ~MycorrhizalNetwork()
        {
            // Deregister from events
            EventBus<EventBus.Events.Turns.Infect>.Deregister(onInfect);
            EventBus<Synthesize>.Deregister(onSynthesize);
        }

        /// <summary>
        /// Increase the number of consecutive infects
        /// </summary>
        private void IncreaseConsecutiveInfects() => consecutiveInfects++;

        /// <summary>
        /// Reset the number of consecutive infects
        /// </summary>
        private void ResetConsecutiveInfects() => consecutiveInfects = 0;

        /// <summary>
        /// Infect gains +5% higher base Combat Rating per consecutive use
        /// </summary>
        public override void ApplyMutation(BattleCalculator calculator, WeatherSystem weather, MutationsTracker mutations)
        {
            // Increase the base Combat Rating by 5% per consecutive use
            calculator.IncreaseBaseAdditives(0.05f * consecutiveInfects);
        }

        /// <summary>
        /// Clone the Mutation
        /// </summary>
        public override MutationStrategy Duplicate()
        {
            return new MycorrhizalNetwork();
        }
    }
}
