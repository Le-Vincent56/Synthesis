using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Synthesis.Modifiers.Traits;
using UnityEngine.UI;
using Synthesis.Creatures;

namespace Synthesis
{
    public class ShopManager : MonoBehaviour
    {
        public Player creature;

        public TraitPool mutationPool;
        public List<Button> buyButtons;
        public List<TextMeshProUGUI> traitTexts;
        private List<Trait> displayedTraits = new List<Trait>();
        private List<bool> purchasedTraits = new List<bool>();
        private ShowTraits showTraitsScript;

        // Start is called before the first frame update
        void Start()
        {
            PopulateShop();

            showTraitsScript = FindObjectOfType<ShowTraits>();

            for (int i = 0; i < buyButtons.Count; i++)
            {
                int index = i; 
                buyButtons[i].onClick.RemoveAllListeners(); 
                buyButtons[i].onClick.AddListener(() => BuyTrait(index)); 
            }
        }

        void PopulateShop()
        {
            if (mutationPool == null || mutationPool.traits.Length == 0)
            {
                Debug.LogError("Traitpool is empty");
                return;
            }

            displayedTraits.Clear();
            purchasedTraits.Clear();

            for (int i = 0; i < mutationPool.traits.Length; i++)
            {
                Trait randomTrait = mutationPool.GetRandomTrait();
                while (displayedTraits.Contains(randomTrait))
                {
                    randomTrait = mutationPool.GetRandomTrait();
                }

                displayedTraits.Add(randomTrait);
                purchasedTraits.Add(false);
                traitTexts[i].text = randomTrait != null ? randomTrait.Name : "No trait Available";
            }
        }

        public void BuyTrait(int index)
        {
            if (index < displayedTraits.Count && displayedTraits[index] != null)
            {
                Trait purchasedTrait = displayedTraits[index];
                Debug.Log("Bought Trait: " + displayedTraits[index].Name);
                purchasedTraits[index] = true;
                traitTexts[index].text = purchasedTrait.Name + " (Bought)";

                if (creature != null)
                {
                    bool success = creature.AddTrait(purchasedTrait); 
                    if (!success)
                    {
                        Debug.LogError("Failed to add trait to creature.");
                    }
                }

                if (showTraitsScript != null)
                {
                    //Debug.Log("Calling ShowTraitsFunction() from ShopManager...");
                    showTraitsScript.ShowTraitsFunction();
                }
                else
                {
                    Debug.LogError("ShowTraits script reference is missing!");
                }
            }
        }
    }
}
