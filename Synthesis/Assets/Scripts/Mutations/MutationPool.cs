using Synthesis.Mutations;
using Synthesis.Mutations.Infect;
using Synthesis.ServiceLocators;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Mutations
{
    public class MutationPool : MonoBehaviour
    {
        private Dictionary<Type, bool> mutationAvailability;
        private List<Type> availableMutations;

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
        /// Get a random available Mutation Type
        /// </summary>
        public Type GetRandomAvailableMutation()
        {
            // Exit case - there are no available Mutations
            if (availableMutations.Count == 0) return null;

            return availableMutations[UnityEngine.Random.Range(0, availableMutations.Count)];
        }

        /// <summary>
        /// Set Mutation availability
        /// </summary>
        public void SetMutationAvailability<T>(bool isAvailable) where T: MutationStrategy
        {
            // Extract the Type of the Mutation Strategy
            Type mutationType = typeof(T);

            // Exit case - the Dictionary does not contain the mutation type
            if (!mutationAvailability.ContainsKey(mutationType)) return;

            // CHange the Mutation availability
            mutationAvailability[mutationType] = isAvailable;

            // Check if the Mutation is available
            if (isAvailable)
                // Add it to the available Mutations list
                availableMutations.Add(mutationType);
            else
                // Remove it from the available Mutations list
                availableMutations.Remove(mutationType);
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
