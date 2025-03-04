using Sirenix.OdinInspector;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using System.Collections.Generic;
using UnityEngine;
using Synthesis.ServiceLocators;
using System;

namespace Synthesis.Mutations
{
    
    public class MutationsTracker : SerializedMonoBehaviour
    {
        private MutationPool mutationPool;
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

        private void Start()
        {
            // Get the Mutation Pool
            mutationPool = ServiceLocator.ForSceneOf(this).Get<MutationPool>();
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
            }

            // Add the Mutations to gain
            for (int i = 0; i < numberToGain; i++)
            {
                // Get a random Mutation to add
                MutationStrategy mutation = mutationPool.GetRandomMutation();

                // Add the Mutation
                mutations.Add(mutation);

                // Set the Mutation's index in the Tracker
                mutation.TrackerIndex = mutations.Count - 1;
            }
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
