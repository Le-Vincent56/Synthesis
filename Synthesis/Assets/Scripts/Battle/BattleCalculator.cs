using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Mutations;
using Synthesis.ServiceLocators;
using Synthesis.Weather;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Battle
{
    public enum Action
    {
        Infect,
        Synthesize
    }

    public class BattleCalculator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MutationsTracker mutationsTracker;
        [SerializeField] private WeatherSystem weatherSystem;

        [Header("General")]
        [SerializeField] private float baseCombatRating = 10f;
        [SerializeField] private float bcrAdditives;
        [SerializeField] private float bcrMultipliers;
        [SerializeField] private float fcrAdditives;
        [SerializeField] private float incomingWiltPercentage = 1f;
        [SerializeField] private float bcrPermanentAdditives = 0f;

        [Header("Actions")]
        [SerializeField] private Action lastAction;
        [SerializeField] private int infectsSinceStartOfBattle;

        private EventBinding<Infect> onInfect;
        private EventBinding<Synthesize> onSynthesize;
        private EventBinding<StartBattle> onStartBattle;

        public Action LastAction { get => lastAction; }
        public int InfectsSinceStartofBattle { get => infectsSinceStartOfBattle; }

        private void Awake()
        {
            // Register this as a service
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void OnEnable()
        {
            onInfect = new EventBinding<Infect>(OnInfect);
            EventBus<Infect>.Register(onInfect);

            onSynthesize = new EventBinding<Synthesize>(OnSynthesize);
            EventBus<Synthesize>.Register(onSynthesize);

            onStartBattle = new EventBinding<StartBattle>(SetStartOfBattleVariables);
            EventBus<StartBattle>.Register(onStartBattle);
        }

        private void OnDisable()
        {
            EventBus<Infect>.Deregister(onInfect);
            EventBus<Synthesize>.Deregister(onSynthesize);
            EventBus<StartBattle>.Deregister(onStartBattle);
        }

        private void Start()
        {
            // Get services
            mutationsTracker = ServiceLocator.ForSceneOf(this).Get<MutationsTracker>();
            weatherSystem = ServiceLocator.ForSceneOf(this).Get<WeatherSystem>();
        }

        /// <summary>
        /// Increase the base Combat Rating by an additive percentage
        /// </summary>
        public void IncreaseBaseAdditives(float bcrAdditive) => bcrAdditives += bcrAdditive;

        /// <summary>
        /// Increase the base Combat Rating by a multiplicative percentage
        /// </summary>
        public void IncreaseBaseMultipliers(float bcrMultiplier) => bcrMultipliers += bcrMultiplier;

        /// <summary>
        /// Increase the final Combat Rating by an additive percentage
        /// </summary>
        public void IncreaseFinalAdditives(float fcrAdditive) => fcrAdditives += fcrAdditive;

        /// <summary>
        /// Increase the base combat rating permanently
        /// </summary>
        public void IncreaseBasePermenentAdditive(float bcrPermanentAdditive) => bcrPermanentAdditives += bcrPermanentAdditive;

        /// <summary>
        /// Increase the base combat rating permanently by a percentage
        /// </summary>
        public void IncreaseBasePermanentPercentage(float bcrPermanentPercentage) => bcrPermanentAdditives += baseCombatRating * bcrPermanentPercentage;

        /// <summary>
        /// Calculate the player's Combat Rating for this turn
        /// </summary>
        public int CalculatePoints()
        {
            // Set base values
            float baseRating = baseCombatRating + bcrPermanentAdditives;
            bcrAdditives = 0f;
            bcrMultipliers = 0f;
            fcrAdditives = 0f;

            // Get the List of Mutations that the Player currently has
            List<MutationStrategy> activeMutations = mutationsTracker.ActiveMutations;

            // Iterate through each Mutation
            foreach (MutationStrategy mutation in activeMutations)
            {
                // Apply the Mutation's effects
                mutation.ApplyMutation(this, weatherSystem, mutationsTracker);
            }

            // Calculate the Final Combat Rating
            float finalRating = baseRating + (baseRating * bcrAdditives);   // Add the additive percentage to the base Combat Rating
            finalRating += baseRating * bcrMultipliers;                     // Add the multiplicative percentage to the base Combat Rating
            finalRating += finalRating * fcrAdditives;                      // Add the additive percentage to the final Combat Rating
            int finalRatingInt = (int)Mathf.Round(finalRating);             // Round the final Combat Rating

            return finalRatingInt;
        }

        /// <summary>
        /// Set the last action to Infect
        /// </summary>
        private void OnInfect()
        {
            // Set the last action
            lastAction = Action.Infect;

            // Increment the number of Infects since the start of the battle
            infectsSinceStartOfBattle++;
        }

        /// <summary>
        /// Set the last action to Synthesize
        /// </summary>
        private void OnSynthesize() => lastAction = Action.Synthesize;

        private void SetStartOfBattleVariables()
        {
            // Reset the number of infects since the start of the battle
            infectsSinceStartOfBattle = 0;
        }
    }
}
