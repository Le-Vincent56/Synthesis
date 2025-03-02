using Synthesis.Mutations;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis
{
    public class MutationsTracker : MonoBehaviour
    {
        private List<MutationStrategy> mutations;

        public List<MutationStrategy> Mutations { get => mutations; }

        private void Awake()
        {
            // Initialize the list of Mutations
            mutations = new List<MutationStrategy>();
        }

        /// <summary>
        /// Add a Mutation to the Mutations Tracker
        /// </summary>
        public void AddMutation(MutationStrategy mutation)
        {
            // Add the Mutation
            mutations.Add(mutation);

            // Set the Mutation's index in the Tracker
            mutation.TrackerIndex = mutations.Count - 1;
        }

        /// <summary>
        /// Remove a Mutation from the Mutations Tracker through reference
        /// </summary>
        public void RemoveMutation(MutationStrategy mutation) => mutations.Add(mutation);

        /// <summary>
        /// Remove a Mutation from the Mutations Tracker through index
        /// </summary>
        public void RemoveMutation(int index) => mutations.RemoveAt(index);
    }
}
