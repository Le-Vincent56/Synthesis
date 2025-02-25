using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Synthesis.Creatures;
using Synthesis.Modifiers.Traits;
using TMPro;

namespace Synthesis
{
    public class OnHoverPopup : MonoBehaviour
    {
        public GameObject popupPanel; // UI panel containing description text
        private Player creature; 
        public TraitPool traitPool;

        public Button expandLimbsButton, electricStrikeButton, fireBoltButton, waterWaveButton;
        public Button buyButton1, buyButton2, buyButton3, buyButton4;
        private TMP_Text descriptionText;

        private void Start()
        {
            // Find the Creature
            creature = GameObject.Find("Creature Base").GetComponent<Player>();

            // Find the description popup panel text
            descriptionText = popupPanel.transform.Find("DescriptionText").GetComponent<TMP_Text>();

            // Ensure popup starts hidden
            if (popupPanel != null)
                popupPanel.SetActive(false);

            // Assign event listeners to buttons
            AssignButtonListeners(expandLimbsButton, "Expand Limbs");
            AssignButtonListeners(electricStrikeButton, "Electric Strike");
            AssignButtonListeners(fireBoltButton, "Fire Bolt");
            AssignButtonListeners(waterWaveButton, "Water Wave");

            // Assign event listeners to buy buttons
            AssignButtonListeners(buyButton1, "Expand Limbs");
            AssignButtonListeners(buyButton2, "Electric Strike");
            AssignButtonListeners(buyButton3, "Fire Bolt");
            AssignButtonListeners(buyButton4, "Water Wave");
        }

        private void AssignButtonListeners(Button button, string traitName)
        {
            if (button != null)
            {
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                entryEnter.callback.AddListener((data) => ShowDescription(traitName));
                trigger.triggers.Add(entryEnter);

                EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
                entryExit.callback.AddListener((data) => HideDescription());
                trigger.triggers.Add(entryExit);
            }
        }

        private void ShowDescription(string traitName)
        {
            if (popupPanel != null && traitPool != null)
            {
                popupPanel.SetActive(true);
                Trait trait = traitPool.GetTraitByName(traitName);
                if (trait != null)
                {
                    descriptionText.text = trait.Description;
                }
                else
                {
                    descriptionText.text = "No description available.";
                }
            }
        }

        private void HideDescription()
        {
            if (popupPanel != null)
            {
                popupPanel.SetActive(false);
            }
        }
    }
}