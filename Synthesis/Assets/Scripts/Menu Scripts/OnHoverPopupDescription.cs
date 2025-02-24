using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Synthesis.Creatures;
using Synthesis.Modifiers.Traits;
using TMPro;

namespace Synthesis
{
    public class OnHoverPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject popupPanel; // UI panel containing buttons
        private Creature creature; // Creature associated with this UI element
        public Dictionary<string, Button> traitButtons = new Dictionary<string, Button>();

        private void Start()
        {
            // Find the associated Creature (modify this logic as needed)
            creature = GameObject.Find("Creature Base").GetComponent<Creature>();

            // Hide the popup panel initially
            if (popupPanel != null)
                popupPanel.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (popupPanel != null && creature != null)
            {
                popupPanel.SetActive(true);

                if (eventData.pointerEnter.name == "BaseTraitButton")
                {
                    popupPanel.transform.Find("TraitNameText").GetComponent<TMP_Text>().text = "Base Trait"; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait").Name;
                    popupPanel.transform.Find("DescriptionText").GetComponent<TMP_Text>().text = "This is a basic script that can add a flat value of 5."; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait").Description;
                }
                else if (eventData.pointerEnter.name == "Trait1Button")
                {
                    popupPanel.transform.Find("TraitNameText").GetComponent<TMP_Text>().text ="Basic Trait 1"; // TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait 1").Name;
                    popupPanel.transform.Find("DescriptionText").GetComponent<TMP_Text>().text = "This is a basic script that does nothing.."; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait 1").Description;
                }
                else if (eventData.pointerEnter.name == "Trait2Button")
                {
                    popupPanel.transform.Find("TraitNameText").GetComponent<TMP_Text>().text = "Basic Trait 2"; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait 2").Name;
                    popupPanel.transform.Find("DescriptionText").GetComponent<TMP_Text>().text = "This is a basic script that adds a multiplier of x0.3333."; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait 2").Description;
                }
                else if (eventData.pointerEnter.name == "Trait3Button")
                {
                    popupPanel.transform.Find("TraitNameText").GetComponent<TMP_Text>().text = "Basic Trait 3"; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait 3").Name;
                    popupPanel.transform.Find("DescriptionText").GetComponent<TMP_Text>().text = "This is a basic script that can add a flat value of 2 and a multiplier of x1.5."; //TraitPoolManager.Instance.traitPool.GetTraitByName("Basic Trait 3").Description;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (popupPanel != null)
            {
                popupPanel.SetActive(false);
            }
        }
    }
}