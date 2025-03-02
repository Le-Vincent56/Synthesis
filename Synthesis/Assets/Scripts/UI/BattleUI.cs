using Synthesis.EventBus;
using Synthesis.EventBus.Events.UI;
using Synthesis.Mutations;
using Synthesis.ServiceLocators;
using Synthesis.UI.Controller;
using Synthesis.UI.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.UI
{
    public class BattleUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BattleUIView view;
        [SerializeField] private MutationPool mutationPool;
        private BattleUIController controller;

        private EventBinding<ShowTurnHeader> onShowTurnHeader;
        private EventBinding<HideTurnHeader> onHideTurnHeader;
        private EventBinding<ShowPlayerInfo> onShowPlayerInfo;
        private EventBinding<HidePlayerInfo> onHidePlayerInfo;

        private void OnEnable()
        {
            onShowTurnHeader = new EventBinding<ShowTurnHeader>(ShowTurnHeader);
            EventBus<ShowTurnHeader>.Register(onShowTurnHeader);

            onHideTurnHeader = new EventBinding<HideTurnHeader>(HideTurnHeader);
            EventBus<HideTurnHeader>.Register(onHideTurnHeader);

            onShowPlayerInfo = new EventBinding<ShowPlayerInfo>(ShowPlayerInfo);
            EventBus<ShowPlayerInfo>.Register(onShowPlayerInfo);

            onHidePlayerInfo = new EventBinding<HidePlayerInfo>(HidePlayerInfo);
            EventBus<HidePlayerInfo>.Register(onHidePlayerInfo);
        }

        private void OnDisable()
        {
            EventBus<ShowTurnHeader>.Deregister(onShowTurnHeader);
            EventBus<HideTurnHeader>.Deregister(onHideTurnHeader);
            EventBus<ShowPlayerInfo>.Deregister(onShowPlayerInfo);
            EventBus<HidePlayerInfo>.Deregister(onHidePlayerInfo);
        }

        private void Start()
        {
            mutationPool = ServiceLocator.ForSceneOf(this).Get<MutationPool>();

            controller = new BattleUIController.Builder()
                .WithMutationsPool(mutationPool)
                .Build(view);
        }

        private void ShowTurnHeader(ShowTurnHeader eventData) => view.ShowTurnHeader(eventData.Text);
    }
}
