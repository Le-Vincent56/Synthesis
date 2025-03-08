using System;
using System.Collections.Generic;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using UnityEngine;
using Synthesis.Turns;
using Synthesis.ServiceLocators;
using Random = UnityEngine.Random;

namespace Synthesis
{
    public class SpawnCreaturesEvil : MonoBehaviour
    {
        public GameObject evilCreaturePrefab;
        private List<GameObject> evilCreatures = new List<GameObject>();
        private int index = 0;
        private int lastCheckedRound = 0;
        private TurnSystem turnSystem; // Reference to TurnSystem

        private EventBinding<StartBattle> onStartBattle;

        public int EvilCreaturesCount { get => evilCreatures.Count; }

        private void Awake()
        {
            // Register this as a service
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void Start()
        {
            turnSystem = FindObjectOfType<TurnSystem>(); // Find the TurnSystem instance in the scene
            if (turnSystem == null)
            {
                Debug.LogError("TurnSystem not found in the scene!");
            }

            //HandleNewRound(1); // Initialize the first round
        }

        private void OnEnable()
        {
            onStartBattle = new EventBinding<StartBattle>(() =>
            {
                HandleNewRound(turnSystem.CurrentRound);
            });
            EventBus<StartBattle>.Register(onStartBattle);
        }

        private void OnDisable()
        {
            EventBus<StartBattle>.Deregister(onStartBattle);
        }

        /*private void Update()
        {
            if (turnSystem != null && turnSystem.CurrentRound > lastCheckedRound)
            {
                lastCheckedRound = turnSystem.CurrentRound;
                HandleNewRound(lastCheckedRound);
            }
        }*/

        private void HandleNewRound(int round)
        {
            if (round >= 0)
            {
                SpawnEvilCreature();

                if (round > 3)
                {
                    round = 3;
                }

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
            if (EvilCreaturesCount < 4)
            {
                if (evilCreaturePrefab != null)
                {
                    GameObject creature = Instantiate(evilCreaturePrefab, transform.position, transform.rotation);
                    creature.transform.parent = transform;
                    creature.transform.localPosition = new Vector3((3 * (index + 1)), 2.5f, 0);
                    evilCreatures.Add(creature);
                    index++;
                }
                else
                {
                    Debug.LogError("Failed to load CreatureEvil prefab");
                }
            }
        }

        private bool RandomChance(int round)
        {
            return Random.Range(0, 100) < round * 34;
        }

        public void RemoveEvilCreature(int creatureIndex)
        {
            if (creatureIndex >= 0 && creatureIndex < evilCreatures.Count)
            {
                GameObject creatureToRemove = evilCreatures[creatureIndex];
                evilCreatures.RemoveAt(creatureIndex);
                Destroy(creatureToRemove);
                index--;
            }
            else
            {
                Debug.LogWarning("Invalid creature index!");
            }
        }
    }
}
