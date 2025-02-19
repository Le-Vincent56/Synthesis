using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using UnityEngine;
using UnityEngine.UI;

namespace Synthesis
{
    public class StateTester : MonoBehaviour
    {
        [SerializeField] private Button actionButton;
        [SerializeField] private Button mutateButton;

        private void OnEnable()
        {
            // Register listeners to Button clicks
            actionButton.onClick.AddListener(EnterActionState);
            mutateButton.onClick.AddListener(EnterMutateState);
        }

        private void OnDisable()
        {
            // Deregister listeners from Button clicks
            actionButton.onClick.RemoveListener(EnterActionState);
            mutateButton.onClick.RemoveListener(EnterMutateState);
        }

        /// <summary>
        /// Event callback handler to enter the Action sub-state
        /// </summary>
        private void EnterActionState() => EventBus<EnterAction>.Raise(new EnterAction());

        /// <summary>
        /// Event callback handler to enter the Mutate sub-state
        /// </summary>
        private void EnterMutateState() => EventBus<EnterMutate>.Raise(new EnterMutate());
    }
}
