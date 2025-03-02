using Synthesis.Mutations;
using Synthesis.UI.Model;
using Synthesis.UI.View;

namespace Synthesis.UI.Controller
{
    public class BattleUIController
    {
        public class Builder
        {
            private readonly BattleUIModel model = new BattleUIModel();

            public Builder WithMutationsPool(MutationPool mutationPool)
            {
                model.SetMutationPool(mutationPool);
                return this;
            }

            public BattleUIController Build(BattleUIView view)
            {
                return new BattleUIController(model, view);
            }
        }

        private BattleUIModel model;
        private BattleUIView view;

        public BattleUIController(BattleUIModel model, BattleUIView view)
        {
            // Set the model and view
            this.model = model;
            this.view = view;
        }

        private void ConnectModel()
        {

        }

        private void ConnectView()
        {

        }
    }
}
