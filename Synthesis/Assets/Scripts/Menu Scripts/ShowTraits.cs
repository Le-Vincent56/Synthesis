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
        private Player creature;
        private Button expandLimbsButton;
        private Button electricStrikeButton;
        private Button fireBoltButton;
        private Button waterWaveButton;

        private void Start()
        {
            // Find the Creature component
            creature = GameObject.Find("Creature Base").GetComponent<Player>();

            //get trait buttons to hover over
            expandLimbsButton = transform.Find("ExpandLimbsButton").GetComponent<Button>();
            electricStrikeButton = transform.Find("ElectricStrikeButton").GetComponent<Button>();
            fireBoltButton = transform.Find("FireBoltButton").GetComponent<Button>();
            waterWaveButton = transform.Find("WaterWaveButton").GetComponent<Button>();

            ShowTraitsFunction();
        }

        public void ShowTraitsFunction()
        {
            foreach (var trait in creature.Infect.Traits)
            {
                //Debug.Log($"Trait found: {trait.Name}");
                switch (trait.Name)
                {
                    case "Expand Limbs":
                        expandLimbsButton.gameObject.SetActive(true);
                        break;
                    case "Electric Strike":
                        electricStrikeButton.gameObject.SetActive(true);
                        break;
                    case "Fire Bolt":
                        fireBoltButton.gameObject.SetActive(true);
                        break;
                    case "Water Wave":
                        waterWaveButton.gameObject.SetActive(true);
                        break;
                    default:
                        Debug.LogWarning($"No button found for trait: {trait.Name}");
                        break;
                }
            }

            foreach (var trait in creature.Synthesize.Traits)
            {
                //Debug.Log($"Trait found: {trait.Name}");
                switch (trait.Name)
                {
                    case "Expand Limbs":
                        expandLimbsButton.gameObject.SetActive(true);
                        break;
                    case "Electric Strike":
                        electricStrikeButton.gameObject.SetActive(true);
                        break;
                    case "Fire Bolt":
                        fireBoltButton.gameObject.SetActive(true);
                        break;
                    case "Water Wave":
                        waterWaveButton.gameObject.SetActive(true);
                        break;
                    default:
                        Debug.LogWarning($"No button found for trait: {trait.Name}");
                        break;
                }
            }
        }
    }
}
