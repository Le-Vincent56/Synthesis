using System.Collections.Generic;
using UnityEngine;
using Synthesis.Turns;

namespace Synthesis
{
    public class SpawnCreaturesEvil : MonoBehaviour
    {
        public GameObject evilCreaturePrefab;
        private List<GameObject> evilCreatures = new List<GameObject>();
        private int index = 0;
        private int lastCheckedRound = 0;
        private TurnSystem turnSystem; // Reference to TurnSystem

        private void Start()
        {
            turnSystem = FindObjectOfType<TurnSystem>(); // Find the TurnSystem instance in the scene
            if (turnSystem == null)
            {
                Debug.LogError("TurnSystem not found in the scene!");
            }

            HandleNewRound(1); // Initialize the first round
        }

        private void Update()
        {
            if (turnSystem != null && turnSystem.CurrentRound > lastCheckedRound)
            {
                lastCheckedRound = turnSystem.CurrentRound;
                HandleNewRound(lastCheckedRound);
            }
        }

        private void HandleNewRound(int round)
        {
            if (round >= 1)
            {
                SpawnEvilCreature();

                for (int i = 1; i < round; i++)
                {
                    if (RandomChance(round))
                    {
                        SpawnEvilCreature();
                    }
                }
            }
        }

        private void SpawnEvilCreature()
        {
            if (evilCreaturePrefab != null)
            {
                GameObject creature = Instantiate(evilCreaturePrefab, transform.position, transform.rotation);
                creature.transform.parent = transform;
                creature.transform.localPosition = new Vector3((4 * (index + 1)), 2.5f, 0);
                evilCreatures.Add(creature);
                index++;
            }
            else
            {
                Debug.LogError("Failed to load CreatureEvil prefab");
            }
        }

        private bool RandomChance(int round)
        {
            return Random.Range(0, 100) < round * 34;
        }
    }
}
