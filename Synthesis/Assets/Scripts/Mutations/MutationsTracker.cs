using Sirenix.OdinInspector;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using System.Collections.Generic;
using UnityEngine;
using Synthesis.ServiceLocators;
using System;
using Synthesis.Battle;
using Synthesis.Weather;

namespace Synthesis.Mutations
{
    
    public class MutationsTracker : SerializedMonoBehaviour
    {
        private BattleCalculator battleCalculator;
        private WeatherSystem weatherSystem;
        private MutationPool mutationPool;

        private bool canFindMultiples;
        [SerializeField] private List<MutationStrategy> mutations;
        [SerializeField] private List<MutationStrategy> activeMutations;
        [SerializeField] private List<MutationStrategy> passiveMutations;

        public List<MutationStrategy> Mutations { get => mutations; }
        public List<MutationStrategy> ActiveMutations { get => activeMutations; }
        public List<MutationStrategy> PassiveMutations { get => passiveMutations; }

        private EventBinding<Synthesize> onSynthesize;

        private void Awake()
        {
            // Initialize the list of Mutations
            mutations = new List<MutationStrategy>();
            activeMutations = new List<MutationStrategy>();
            passiveMutations = new List<MutationStrategy>();

            // Don't allow multiples by default
            canFindMultiples = false;

            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void OnEnable()
        {
            onSynthesize = new EventBinding<Synthesize>(AddMutation);
            EventBus<Synthesize>.Register(onSynthesize);
        }

        private void OnDisable()
        {
            EventBus<Synthesize>.Deregister(onSynthesize);
        }

        private void Start()
        {
            // Get services
            mutationPool = ServiceLocator.ForSceneOf(this).Get<MutationPool>();
            battleCalculator = ServiceLocator.ForSceneOf(this).Get<BattleCalculator>();
            weatherSystem = ServiceLocator.ForSceneOf(this).Get<WeatherSystem>();
        }

        /// <summary>
        /// Reroll a certain amount of Mutations for a certain nubmer of Mutations
        /// </summary>
        public void Reroll(int numberToLose, int numberToGain, List<Type> lossExceptions = null)
        {
            // Remove the Mutations to lose
            for (int i = 0; i < numberToLose; i++)
            {
                // Get a random index to remove
                int index = UnityEngine.Random.Range(0, mutations.Count);

                // Get the Mutation at the index
                MutationStrategy mutation = mutations[index];

                // Check if there are Mutation exceptions
                if(lossExceptions != null)
                {
                    // Set a default true for exceptions being found
                    bool isException = true;
                    
                    // While there is an exception
                    while(isException)
                    {
                        // Check if the Mutation is an exception
                        if (lossExceptions.Contains(mutation.GetType()))
                        {
                            // If so, get a new Mutation
                            index = UnityEngine.Random.Range(0, mutations.Count);
                            mutation = mutations[index];
                        }
                        else
                        {
                            // Otherwise, exit the loop
                            isException = false;
                        }
                    }
                }

                // Remove the Mutation
                mutations.RemoveAt(index);

                // Check if the Mutation is in the active Mutations list
                if (activeMutations.Contains(mutation))
                    // Remove it
                    activeMutations.Remove(mutation);

                // Check if the Mutation is in the passive Mutations list
                if (passiveMutations.Contains(mutation))
                    // Remove it
                    passiveMutations.Remove(mutation);
            }

            // Add the Mutations to gain
            for (int i = 0; i < numberToGain; i++)
            {
                // Add a random Mutation
                AddMutation(mutationPool.GetRandomMutation());
            }
        }

        /// <summary>
        /// Event callback to add a Mutation to the Mutations Tracker
        /// </summary>
        public void AddMutation(Synthesize eventData)
        {
            // Add the Mutation from Synthesize
            AddMutation(eventData.Mutation, true);

            // Iterate through each Passive Mutation
            foreach(MutationStrategy mutation in passiveMutations)
            {
                // Apply the Mutation
                mutation.ApplyMutation(battleCalculator, weatherSystem, this);
            }
        }

        /// <summary>
        /// Add a Mutation to the Mutations Tracker
        /// </summary>
        private void AddMutation(MutationStrategy newMutation, bool fromSynthesize = false)
        {
            MutationStrategy mutation = newMutation;

            // Add the Mutation
            mutations.Add(mutation);

            // Check if its an Active Mutation
            if (mutation.MutationType == MutationType.Active)
                // Add it to the Active Mutations list
                activeMutations.Add(mutation);

            // Check if its a Passive Mutation
            if (mutation.MutationType == MutationType.Passive)
                // Add it to the Passive Mutations list
                passiveMutations.Add(mutation);

            // Set the Mutation's index in the Tracker
            mutation.TrackerIndex = mutations.Count - 1;

            // Exit case - if the Mutation is not from Synthesize
            if (!fromSynthesize) return;

            // Exit case - if the Player can find multiple of this Mutation
            if (canFindMultiples) return;

            // Set the Mutation's availability to unavailable
            mutationPool.SetMutationAvailability(mutation.GetType(), false);
        }

        /// <summary>
        /// Remove a Mutation from the Mutations Tracker through reference
        /// </summary>
        public void RemoveMutation(MutationStrategy mutation)
        {
            // Finalize the Mutation's removal
            mutation.OnRemove(battleCalculator, weatherSystem, this);

            // Remove the Mutations from the Tracker
            mutations.Remove(mutation);

            // Check if the Mutation is in the active Mutations list
            if (activeMutations.Contains(mutation))
                // Remove it
                activeMutations.Remove(mutation);

            // Check if the Mutation is in the passive Mutations list
            if (passiveMutations.Contains(mutation))
                // Remove it
                passiveMutations.Remove(mutation);
        }

        /// <summary>
        /// Remove a Mutation from the Mutations Tracker through index
        /// </summary>
        public void RemoveMutation(int index)
        {
            // Get the Mutation at the index
            MutationStrategy mutationToRemove = mutations[index];

            // Remove the Mutation
            RemoveMutation(mutationToRemove);
        }

        /// <summary>
        /// Duplicate a random Mutation Strategy
        /// </summary>
        public void DuplicateRandom()
        {
            // Exit case - if there are no Mutations
            if (mutations.Count == 0) return;

            // Get a random Mutation
            MutationStrategy mutation = mutations[UnityEngine.Random.Range(0, mutations.Count)];

            // Duplicate the Mutation
            MutationStrategy duplicate = mutation.Duplicate();

            // Add the Duplicate
            AddMutation(duplicate);
        }

        /// <summary>
        /// Allow the Player to find multiples of Mutations
        /// </summary>
        public void AllowMultiples()
        {
            // Set the Player to find multiples of Mutations
            canFindMultiples = true;

            // Iterate through each Mutation
            foreach (MutationStrategy mutation in mutations)
            {
                // Set the Mutation's availability to available
                mutationPool.SetMutationAvailability(mutation.GetType(), true);
            }
        }

        /// <summary>
        /// Disallow the Player to find multiples of Mutations
        /// </summary>
        public void DisallowMultiples()
        {
            // Set the Player to not find multiples of Mutations
            canFindMultiples = false;

            // Iterate through each Mutation
            foreach (MutationStrategy mutation in mutations)
            {
                // Set the Mutation's availability to unavailable
                mutationPool.SetMutationAvailability(mutation.GetType(), false);
            }
        }
    }
}
