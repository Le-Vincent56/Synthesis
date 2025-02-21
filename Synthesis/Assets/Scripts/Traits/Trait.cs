using System;
using System.Collections;
using System.Collections.Generic;
using Synthesis.Traits;
using UnityEngine;

namespace Synthesis.Traits
{
    [CreateAssetMenu(fileName = "Basic Trait", menuName = "ScriptableObjects/Traits", order = 1)]
    // Container for Trait Strategies.
    public class Trait : ScriptableObject, ITrait
    {

        private MoveType type = MoveType.Both;
        private string traitName = "Trait";
        [SerializeField] protected float additive = 0;
        [SerializeField] protected float multiplier = 1;

        public MoveType Type { get => type; }
        public string Name { get => traitName; }

        public virtual void Activate(ref MoveInfo info)
        {
            info.Additive += additive;
            info.Multiplier *= multiplier;
        }
    }
    
    public interface ITrait
    {
        // Used to check if the move can be added to specific trait list.
        public MoveType Type { get; }
        
        // Name of Trait
        public string Name { get; }

        // Do any logic in here.
        public void Activate(ref MoveInfo info);
    }
}
