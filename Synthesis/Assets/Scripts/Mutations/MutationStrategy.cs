using Synthesis.Calculator;
using Synthesis.Weather;
using System;
using UnityEngine;

namespace Synthesis.Mutations
{
    [Serializable]
    public abstract class MutationStrategy
    {
        [SerializeField] protected int trackerIndex;
        [SerializeField] protected string name;
        [SerializeField] protected string description;

        public string Name { get => name; }
        public string Description { get => description; }

        public int TrackerIndex { get => trackerIndex; set => trackerIndex = value; }

        public abstract void ApplyMutation(BattleCalculator calculator, WeatherSystem weather);
    }
}
