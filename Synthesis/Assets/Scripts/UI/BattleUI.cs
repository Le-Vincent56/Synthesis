using Synthesis.EventBus;
using Synthesis.EventBus.Events.Battle;
using Synthesis.EventBus.Events.Mutations;
using Synthesis.EventBus.Events.Turns;
using Synthesis.EventBus.Events.UI;
using Synthesis.Mutations;
using Synthesis.ServiceLocators;
using Synthesis.UI.Controller;
using Synthesis.UI.View;
using UnityEngine;

namespace Synthesis.UI
{
    public class BattleUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MutationCard mutationCardPrefab;
        [SerializeField] private Transform mutationCardParent;
        [SerializeField] private BattleUIView view;
        private MutationPool mutationPool;
        private BattleUIController controller;

        private EventBinding<ShowTurnHeader> onShowTurnHeader;
        private EventBinding<HideTurnHeader> onHideTurnHeader;
        private EventBinding<UpdateTurns> onUpdateTurns;
        private EventBinding<BattleMetricsSet> onBattleMetricsSet;
        private EventBinding<FesterCalculated> onCombatRatingCalculated;
        private EventBinding<FesterFinalized> onCombatRatingFinalized;
        private EventBinding<ShowPlayerInfo> onShowPlayerInfo;
        private EventBinding<HidePlayerInfo> onHidePlayerInfo;
        private EventBinding<ShowSynthesizeShop> onShowSynthesizeShop;
        private EventBinding<HideSynthesizeShop> onHideSynthesizeShop;
        private EventBinding<WiltApplied> onWiltApplied;
        private EventBinding<SetCanSynthesize> onSynthesize;
        private EventBinding<Synthesize> onAddMutation;

        private void OnEnable()
        {
            onShowTurnHeader = new EventBinding<ShowTurnHeader>(ShowTurnHeader);
            EventBus<ShowTurnHeader>.Register(onShowTurnHeader);

            onHideTurnHeader = new EventBinding<HideTurnHeader>(HideTurnHeader);
            EventBus<HideTurnHeader>.Register(onHideTurnHeader);

            onUpdateTurns = new EventBinding<UpdateTurns>(UpdateTurns);
            EventBus<UpdateTurns>.Register(onUpdateTurns);

            onBattleMetricsSet = new EventBinding<BattleMetricsSet>(BattleMetricsSet);
            EventBus<BattleMetricsSet>.Register(onBattleMetricsSet);

            onCombatRatingCalculated = new EventBinding<FesterCalculated>(UpdateCombatRatingDisplay);
            EventBus<FesterCalculated>.Register(onCombatRatingCalculated);

            onCombatRatingFinalized = new EventBinding<FesterFinalized>(UpdateCurrentCombatRating);
            EventBus<FesterFinalized>.Register(onCombatRatingFinalized);

            onShowPlayerInfo = new EventBinding<ShowPlayerInfo>(ShowPlayerInfo);
            EventBus<ShowPlayerInfo>.Register(onShowPlayerInfo);

            onHidePlayerInfo = new EventBinding<HidePlayerInfo>(HidePlayerInfo);
            EventBus<HidePlayerInfo>.Register(onHidePlayerInfo);

            onShowSynthesizeShop = new EventBinding<ShowSynthesizeShop>(ShowSynthesizeShop);
            EventBus<ShowSynthesizeShop>.Register(onShowSynthesizeShop);

            onHideSynthesizeShop = new EventBinding<HideSynthesizeShop>(HideSynthesizeShop);
            EventBus<HideSynthesizeShop>.Register(onHideSynthesizeShop);

            onWiltApplied = new EventBinding<WiltApplied>(UpdateWilt);
            EventBus<WiltApplied>.Register(onWiltApplied);

            onSynthesize = new EventBinding<SetCanSynthesize>(CheckCanSynthesize);
            EventBus<SetCanSynthesize>.Register(onSynthesize);

            onAddMutation = new EventBinding<Synthesize>(AddMutationTag);
            EventBus<Synthesize>.Register(onAddMutation);
        }

        private void OnDisable()
        {
            EventBus<ShowTurnHeader>.Deregister(onShowTurnHeader);
            EventBus<HideTurnHeader>.Deregister(onHideTurnHeader);
            EventBus<UpdateTurns>.Deregister(onUpdateTurns);
            EventBus<BattleMetricsSet>.Deregister(onBattleMetricsSet);
            EventBus<FesterCalculated>.Deregister(onCombatRatingCalculated);
            EventBus<FesterFinalized>.Deregister(onCombatRatingFinalized);
            EventBus<ShowPlayerInfo>.Deregister(onShowPlayerInfo);
            EventBus<HidePlayerInfo>.Deregister(onHidePlayerInfo);
            EventBus<ShowSynthesizeShop>.Deregister(onShowSynthesizeShop);
            EventBus<HideSynthesizeShop>.Deregister(onHideSynthesizeShop);
            EventBus<WiltApplied>.Deregister(onWiltApplied);
            EventBus<SetCanSynthesize>.Deregister(onSynthesize);
            EventBus<Synthesize>.Deregister(onAddMutation);
        }

        private void Start()
        {
            mutationPool = ServiceLocator.ForSceneOf(this).Get<MutationPool>();

            // Build the controller
            controller = new BattleUIController.Builder()
                .WithMutationsPool(mutationPool)
                .WithMutationCardPool(mutationCardPrefab, mutationCardParent)
                .Build(view);
        }

        private void ShowTurnHeader(ShowTurnHeader eventData) => controller.ShowTurnHeader(eventData.Text);
        private void HideTurnHeader() => controller.HideTurnHeader();
        private void UpdateTurns(UpdateTurns eventData) => controller.UpdateTurns(eventData.CurrentTurn, eventData.TotalTurns);
        private void BattleMetricsSet(BattleMetricsSet eventData)
        {
            controller.SetBattleMetrics(
                eventData.CurrentFester,
                eventData.TargetFester,
                eventData.CurrentWilt,
                eventData.TotalWilt
            );
        }
        private void UpdateCombatRatingDisplay(FesterCalculated eventData) => controller.UpdateCombatRatingDisplay(eventData.CalculatedFester, eventData.CurrentFester, eventData.TargetFester);
        private void UpdateCurrentCombatRating(FesterFinalized eventData) => controller.UpdateCurrentCombatRating(eventData.CalculatedFester, eventData.TargetFester);
        private void ShowPlayerInfo() => controller.ShowPlayerInfo();
        private void HidePlayerInfo() => controller.HidePlayerInfo();
        private void ShowSynthesizeShop() => controller.ShowSynthesizeShop();
        private void HideSynthesizeShop() => controller.HideSynthesizeShop();
        private void UpdateWilt(WiltApplied eventData) => controller.UpdateWilt(eventData.CurrentWilt, eventData.TotalWilt);

        private void CheckCanSynthesize(SetCanSynthesize eventData) => controller.SetCanSynthesize(eventData.CanSynthesize);

        private void AddMutationTag(Synthesize eventData) => view.AddMutationTag(eventData);
    }
}
