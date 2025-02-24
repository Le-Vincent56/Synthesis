using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Synthesis.Creatures;
using Synthesis.Modifiers.Traits;

namespace Synthesis
{
    public class ShowTraits : MonoBehaviour
    {
        //public GameObject popupPanel; 
        private Creature creature;
        private Button baseTraitButton;
        private Button trait1Button;
        private Button trait2Button;
        private Button trait3Button;

        private void Start()
        {
            // Find the Creature component
            creature = GameObject.Find("Creature Base").GetComponent<Creature>();

            //get trait buttons to hover over
            baseTraitButton = transform.Find("BaseTraitButton").GetComponent<Button>();
            trait1Button = transform.Find("Trait1Button").GetComponent<Button>();
            trait2Button = transform.Find("Trait2Button").GetComponent<Button>();
            trait3Button = transform.Find("Trait3Button").GetComponent<Button>();

            foreach (var move in creature.moves)
            {
                foreach (var trait in move.Traits)
                {
                    //Debug.Log($"Trait found: {trait.Name}");
                    switch (trait.Name)
                    {
                        case "Basic Trait":
                            baseTraitButton.gameObject.SetActive(true);
                            break;
                        case "Basic Trait 1":
                            trait1Button.gameObject.SetActive(true);
                            break;
                        case "Basic Trait 2":
                            trait2Button.gameObject.SetActive(true);
                            break;
                        case "Basic Trait 3":
                            trait3Button.gameObject.SetActive(true);
                            break;
                        default:
                            Debug.LogWarning($"No button found for trait: {trait.Name}");
                            break;
                    }
                }
            }
        }
    }
}
