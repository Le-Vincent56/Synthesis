using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Mutations;
using Synthesis.Mutations.Infect;
using Synthesis.ServiceLocators;
using System;
using System.Collections.Generic;
using Synthesis.Creatures.Visual;
using UnityEngine;
using Synthesis.EventBus.Events.Mutations;

namespace Synthesis.Mutations
{
    
    public class MutationPool : MonoBehaviour
    {
        private Dictionary<Type, bool> mutationAvailability;
        private List<Type> availableMutations;

        private EventBinding<Synthesize> onSynthesize;
        

        private void Awake()
        {
            // Initialize data sets
            mutationAvailability = new Dictionary<Type, bool>();
            availableMutations = new List<Type>();

            // Fill the Mutation Pool
            FillPool();

            // Register this as a service
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void OnEnable()
        {
            onSynthesize = new EventBinding<Synthesize>(OnSynthesize);
            EventBus<Synthesize>.Register(onSynthesize);
        }

        private void OnDisable()
        {
            EventBus<Synthesize>.Deregister(onSynthesize);
        }

        /// <summary>
        /// Fill the Mutation Pool with all mutations
        /// </summary>
        private void FillPool()
        {
            // Register all Infect mutations
            RegisterMutation<PhotosyntheticAmplification>();
            RegisterMutation<VerdantFlood>();
            RegisterMutation<MonsoonBloom>();
            RegisterMutation<ThermalSepta>();
            RegisterMutation<MycorrhizalNetwork>();
            RegisterMutation<AdaptiveChloroplast>();
            RegisterMutation<RecursiveSpores>();
            RegisterMutation<CataclyticBurst>();
            RegisterMutation<UnstableMutagen>();
        }

        /// <summary>
        /// Set Mutation Availability after the player gains a Mutation by Synthesizing
        /// </summary>
        private void OnSynthesize(Synthesize eventData)
        {
            // Extract the Type of the Mutation
            Type mutationType = eventData.Mutation.GetType();

            // Set its availability to false
            SetMutationAvailability(mutationType, false);
        }

        /// <summary>
        /// Register a Mutation within the Mutation Pool
        /// </summary>
        private void RegisterMutation<T>() where T : MutationStrategy
        {
            // Extract the Mutation Type
            Type mutationType = typeof(T);

            // Exit case - the mutation is already registered
            if (mutationAvailability.ContainsKey(mutationType)) return;

            // Register the Mutation
            mutationAvailability.Add(mutationType, true);
            availableMutations.Add(mutationType);
        }

        /// <summary>
        /// Gets a List of unique Mutations up to a given number
        /// </summary>
        private List<Type> GetUniqueMutations(int count)
        {
            // Ensure we do not request more than what is available
            int maxCount = Mathf.Min(count, availableMutations.Count);

            // Create a copy of the available mutations and shuffle it to randomize order
            List<Type> shuffledMutations = new List<Type>(availableMutations);
            ShuffleList(shuffledMutations);

            // Create the final list
            List<Type> selectedMutations = new List<Type>();

            // Iterate a number of ties equal to the given max count
            for (int i = 0; i < maxCount; i++)
            {
                // Add the selected Mutation to the list
                selectedMutations.Add(shuffledMutations[i]);
            }

            return selectedMutations;
        }

        /// <summary>
        /// Fisher-Yates Shuffle Algorithm to shuffle a List in place.
        /// </summary>
        private void ShuffleList<T>(List<T> list)
        {
            // Iterate throug the list starting from the back
            for (int i = list.Count - 1; i > 0; i--)
            {
                // Pick a random index from the start to the current index
                int j = UnityEngine.Random.Range(0, i + 1);

                // Swap the list at index i with the list at index j
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Set Mutation availability
        /// </summary>
        public void SetMutationAvailability(Type mutationType, bool isAvailable)
        {
            // Exit case - the Dictionary does not contain the mutation type
            if (!mutationAvailability.ContainsKey(mutationType)) return;

            // CHange the Mutation availability
            mutationAvailability[mutationType] = isAvailable;

            // Check if the Mutation is available
            if (isAvailable)
            {
                // Exit case - the Mutation is already in the available Mutations list
                if (availableMutations.Contains(mutationType)) return;

                // Add it to the available Mutations list
                availableMutations.Add(mutationType);
            }
            else
            {
                // Remove it from the available Mutations list
                availableMutations.Remove(mutationType);
            }

            // Check if the player can synthesize
            EventBus<SetCanSynthesize>.Raise(new SetCanSynthesize() { CanSynthesize = availableMutations.Count > 0 });
        }

        /// <summary>
        /// Get a List of Mutations from a List of Mutation Types
        /// </summary>
        private List<MutationStrategy> GetMutationsFromType(List<Type> mutationTypes)
        {
            // Create a list of Mutations
            List<MutationStrategy> mutations = new List<MutationStrategy>();

            // Iterate through each mutation type
            foreach (Type mutationType in mutationTypes)
            {
                // Get the Mutation instance
                MutationStrategy mutation = GetMutationInstance(mutationType);

                // Exit case - the Mutation is null
                if (mutation == null) continue;

                // Add the Mutation to the list
                mutations.Add(mutation);
            }

            return mutations;
        }

        /// <summary>
        /// Get a List of Mutations up to a certain count
        /// </summary>
        public List<MutationStrategy> GetMutations(int count)
        {
            // Get a List of unique Mutation Types
            List<Type> uniqueMutationTypes = GetUniqueMutations(count);

            // Return a List of Mutations from the unique Mutation Types
            return GetMutationsFromType(uniqueMutationTypes);
        }

        /// <summary>
        /// Get a random Mutation
        /// </summary>
        public MutationStrategy GetRandomMutation()
        {
            // Get a random type from the available Mutations
            Type mutationType = availableMutations[UnityEngine.Random.Range(0, availableMutations.Count)];

            // Return an instance of the Mutation
            return GetMutationInstance(mutationType);
        }

        /// <summary>
        /// Get an instance of the Mutation
        /// </summary>
        public MutationStrategy GetMutationInstance(Type mutationType)
        {
            // Exit case - the Type is not a subclass of MutationStrategy
            if (!mutationType.IsSubclassOf(typeof(MutationStrategy))) return null;
            
            return (MutationStrategy)Activator.CreateInstance(mutationType);
        }
    }
}
