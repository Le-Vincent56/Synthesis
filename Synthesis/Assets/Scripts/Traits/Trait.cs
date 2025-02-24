using System;
using System.Collections;
using System.Collections.Generic;
using Synthesis.Creatures.Visual;
using Synthesis.Traits;
using UnityEngine;

namespace Synthesis.Traits
{
    [CreateAssetMenu(fileName = "Basic Trait", menuName = "ScriptableObjects/BasicTrait", order = 1)]
    // Container for Trait Strategies.
    public class Trait : ScriptableObject, ITrait
    {

        private MoveType type = MoveType.Both;

        [SerializeField] [TextArea]
        private string description = "This is a basic script that can add a flat value of {additive} and a multiplier of x{multiplier}.";
        [SerializeField] protected float additive = 0;
        [SerializeField] protected float multiplier = 1;
        [SerializeField] public CreaturePiece associatedPiece;
        [SerializeField] public Color color = Color.white;

        public MoveType Type { get => type; }
        public string Name { get => name; }
        public string Description { get => description; }

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
        
        // Trait Description
        public string Description { get; }

        // Do any logic in here.
        public void Activate(ref MoveInfo info);
    }
}
