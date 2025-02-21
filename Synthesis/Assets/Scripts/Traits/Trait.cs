using System;
using System.Collections;
using System.Collections.Generic;
using Synthesis.Traits;
using UnityEngine;

namespace Synthesis.Traits
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    // Container for Trait Strategies.
    public class Trait : ScriptableObject, ITrait
    {

        private MoveType type = MoveType.Both;
        private string name = "Generic Trait";
        [SerializeField] private TraitStrategy strategy;

        public MoveType Type { get => type; }
        public string Name { get => name; }

        TraitStrategy ITrait.Strategy
        {
            get => strategy;
            set => strategy = value;
        }

        public virtual void Activate(ref MoveInfo info)
        {
            strategy.Activate(ref info);
        }
    }
    
    public interface ITrait
    {
        // Used to check if the move can be added to specific trait list.
        public MoveType Type { get; }
        
        // Name of Trait
        public string Name { get; }

        protected TraitStrategy Strategy { get; set; }

        // Do any logic in here.
        public void Activate(ref MoveInfo info);
    }
}
