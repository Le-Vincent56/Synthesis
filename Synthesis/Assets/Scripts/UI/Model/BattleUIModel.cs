using Synthesis.Mutations;
using Synthesis.Utilities.ObservableList;
using System;
using System.Collections.Generic;

namespace Synthesis.UI.Model
{
    public class BattleUIModel
    {
        private MutationPool mutationPool;
        private readonly ObservableList<MutationData> mutations;
        public ObservableList<MutationData> Mutations { get => mutations; }

        public BattleUIModel()
        {
            // Initialize the list of mutations
            mutations = new ObservableList<MutationData>();
        }

        /// <summary>
        /// Set the Mutation Pool
        /// </summary>
        public void SetMutationPool(MutationPool mutationPool) => this.mutationPool = mutationPool;

        /// <summary>
        /// Add a Mutation Data to the list of mutations
        /// </summary>
        public void Add(MutationData mutation) => mutations.Add(mutation);

        /// <summary>
        /// Get a number of mutations
        /// </summary>
        public List<MutationStrategy> GetMutations(int count)
        {
            // Create a list of traits
            List<MutationStrategy> traits = new List<MutationStrategy>();

            // Iterate as many times as the given count
            for (int i = 0; i < count; i++)
            {
                // Get a random mutation type
                Type mutationType = mutationPool.GetRandomAvailableMutation();

                // Add a random trait to the list
                traits.Add(mutationPool.GetMutationInstance(mutationType));
            }

            return traits;
        }
    }
}
