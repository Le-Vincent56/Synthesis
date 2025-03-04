using Synthesis.Mutations;
using Synthesis.UI.Model;
using Synthesis.UI.View;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.UI.Controller
{
    public class BattleUIController
    {
        public class Builder
        {
            private MutationCardPool mutationCardPool;
            private readonly BattleUIModel model = new BattleUIModel();

            public Builder WithMutationsPool(MutationPool mutationPool)
            {
                model.SetMutationPool(mutationPool);
                return this;
            }

            public Builder WithMutationCardPool(MutationCard prefab, Transform parent)
            {
                mutationCardPool = new MutationCardPool(prefab, parent);
                return this;
            }

            public BattleUIController Build(BattleUIView view)
            {
                return new BattleUIController(model, view, mutationCardPool);
            }
        }

        private BattleUIModel model;
        private BattleUIView view;
        private MutationCardPool mutationCardPool;

        public BattleUIController(BattleUIModel model, BattleUIView view, MutationCardPool mutationCardPool)
        {
            // Set the model and view
            this.model = model;
            this.view = view;
            this.mutationCardPool = mutationCardPool;
        }

        public void ShowTurnHeader(string text) => view.ShowTurnHeader(text);
        public void HideTurnHeader() => view.HideTurnHeader();
        public void UpdateTurns(int currentTurn, int totalTurns) => view.UpdateTurns(currentTurn, totalTurns);
        public void ShowPlayerInfo() => view.ShowPlayerInfo();
        public void HidePlayerInfo() => view.HidePlayerInfo();
        public void ShowSynthesizeShop()
        {
            // Get a list of selected Mutations
            List<MutationStrategy> selectedMutations = model.GetMutations(3);

            // Create a list of Mutation Cards
            List<MutationCard> mutationCards = new List<MutationCard>();

            // Iterate through each selected Mutation
            foreach(MutationStrategy mutation in selectedMutations)
            {
                MutationCard card = mutationCardPool.Get();
                card.SetData(mutation);
                mutationCards.Add(card);
            }

            // Show them in the Shop
            view.ShowSynthesizeShop(mutationCards);
        }
        public void HideSynthesizeShop() => view.HideSynthesizeShop(mutationCardPool);
        public void SetBattleMetrics(int currentCombatRating, int targetCombatRating, int currentWilt, int totalWilt)
        {
            // Update the view
            view.UpdateCurrentCombatRating(currentCombatRating);
            view.UpdateTargetCombatRating(targetCombatRating);
            view.UpdateCurrentWilt(currentWilt, totalWilt);
            view.UpdateTotalWilt(totalWilt);
        }
        public void UpdateCurrentCombatRating(int currentCombatRating) => view.UpdateCurrentCombatRating(currentCombatRating);
        public void UpdateWilt(int currentWilt, int totalWilt) => view.UpdateCurrentWilt(currentWilt, totalWilt);
        public void SetCanSynthesize(bool canSynthesize) => view.SetCanSynthesize(canSynthesize);
    }
}
