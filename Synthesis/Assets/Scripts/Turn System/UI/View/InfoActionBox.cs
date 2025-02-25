using Synthesis.EventBus;
using Synthesis.EventBus.Events.UI;
using TMPro;
using UnityEngine;

namespace Synthesis
{
    public class InfoActionBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText;

        private EventBinding<SetInfoText> onSetInfoText;

        private void OnEnable()
        {
            onSetInfoText = new EventBinding<SetInfoText>(SetInfoText);
            EventBus<SetInfoText>.Register(onSetInfoText);
        }

        private void OnDisable()
        {
            EventBus<SetInfoText>.Deregister(onSetInfoText);
        }

        /// <summary>
        /// Set the Info Text
        /// </summary>
        private void SetInfoText(SetInfoText e)
        {
            infoText.text = e.Text;
        }
    }
}
