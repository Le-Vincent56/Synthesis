using Sirenix.OdinInspector;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using System.Collections.Generic;
using UnityEngine;
using Synthesis.ServiceLocators;

namespace Synthesis.Mutations
{
    
    public class MutationsTracker : SerializedMonoBehaviour
    {
        [SerializeField] private List<MutationStrategy> mutations = new List<MutationStrategy>();

        public List<MutationStrategy> Mutations { get => mutations; }

        private EventBinding<Synthesize> onSynthesize;

        private void Awake()
        {
            // Initialize the list of Mutations
            mutations = new List<MutationStrategy>();

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

        /// <summary>
        /// Add a Mutation to the Mutations Tracker
        /// </summary>
        public void AddMutation(Synthesize eventData)
        {
            // Get the Mutation from the Event Data
            MutationStrategy mutation = eventData.Mutation;

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
