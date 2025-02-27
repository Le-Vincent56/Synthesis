using Synthesis.Utilities.ObservableList;

namespace Synthesis
{
    public class BattleUIModel
    {
        private readonly ObservableList<MutationData> mutations = new ObservableList<MutationData>();
        public ObservableList<MutationData> Mutations { get => mutations; }

        /// <summary>
        /// Add a Mutation Data to the list of mutations
        /// </summary>
        public void Add(MutationData mutation) => mutations.Add(mutation);
    }
}
