using System;
using Synthesis.Creatures.Visual;
using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
    [CreateAssetMenu(fileName = "Basic Trait", menuName = "ScriptableObjects/BasicTrait", order = 1)]
    // Container for Trait Strategies.
    public class Trait : ScriptableObject, ITrait, IModifier
    {
        [SerializeField] protected MoveType type = MoveType.Both;

        [Tooltip("{0} is additive value, {1} is multiplicative value, {2} is name.")]
        [SerializeField] [TextArea]
        protected string description = "{0} adds a flat value of {1} and a multiplier of x{2} to attack damage, a flat value of {3} and a multiplier of x{4} to healing, and a flat value of {5} and a multiplier of x{6} to mutation gain.";
        [SerializeField] protected float additive = 0;
        [SerializeField] protected float multiplier = 1;
        [SerializeField] protected float additiveHeal = 0;
        [SerializeField] protected float multiplierHeal = 1;
        [SerializeField] protected float additiveMutate = 0;
        [SerializeField] protected float multiplierMutate = 1;
        [SerializeField] public CreaturePiece associatedPiece;
        [SerializeField] public Color color = Color.white;

        public MoveType Type { get => type; }
        public string Name { get => name; }

        public virtual string Description
        {
            get
            {
                return String.Format(description, name, additive, multiplier, additiveHeal, multiplierHeal, additiveMutate, multiplierMutate);
            }
        }

        public virtual void ApplyModifier(ref MoveInfo info)
        {
            if (type == MoveType.Attack || type == MoveType.Both)
            {
                info.attack.Additive += additive;
                info.attack.Multiplier *= multiplier;
            }

            if (type == MoveType.Synthesize || type == MoveType.Both)
            {
                info.mutate.Additive += additive;
                info.mutate.Multiplier *= multiplier;
            }
            Debug.Log(Description);
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
    }
}
