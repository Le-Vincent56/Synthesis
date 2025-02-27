using Synthesis.Modifiers.Traits;
using Synthesis.Utilities.ObservableList;
using System.Collections.Generic;

namespace Synthesis.UI.Model
{
    public class BattleUIModel
    {
        private TraitPool traitPool;
        private readonly ObservableList<MutationData> mutations;
        public ObservableList<MutationData> Mutations { get => mutations; }

        public BattleUIModel()
        {
            // Initialize the list of mutations
            mutations = new ObservableList<MutationData>();
        }

        /// <summary>
        /// Set the Trait Pool
        /// </summary>
        public void SetTraitPool(TraitPool traitPool) => this.traitPool = traitPool;

        /// <summary>
        /// Add a Mutation Data to the list of mutations
        /// </summary>
        public void Add(MutationData mutation) => mutations.Add(mutation);

        /// <summary>
        /// Get a number of mutations
        /// </summary>
        public List<Trait> GetMutations(int count)
        {
            // Create a list of traits
            List<Trait> traits = new List<Trait>();

            // Iterate as many times as the given count
            for (int i = 0; i < count; i++)
            {
                // Add a random trait to the list
                traits.Add(traitPool.GetRandomTrait());
            }

            return traits;
        }
    }
}
