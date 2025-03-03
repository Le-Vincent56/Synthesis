using Synthesis.Calculator;
using Synthesis.Weather;
using System;
using Synthesis.Creatures;
using UnityEngine;

namespace Synthesis.Mutations
{
    [Serializable]
    public abstract class MutationStrategy
    {
        [SerializeField] protected int trackerIndex;
        [SerializeField] protected string name;
        [SerializeField] protected string description;
        [SerializeField] protected MutationPartType partType = MutationPartType.Infect;
        [SerializeField] protected Color color0 = Color.white;
        [SerializeField] protected Color color1 = Color.white;
        [SerializeField] protected Color color2 = Color.black;

        public string Name { get => name; }
        public string Description { get => description; }
        public MutationPartType PartType => partType;
        public Color Color0 => color0;
        public Color Color1 => color1;
        public Color Color2 => color2;


        public int TrackerIndex { get => trackerIndex; set => trackerIndex = value; }

        public abstract void ApplyMutation(BattleCalculator calculator, WeatherSystem weather);
    }
}
