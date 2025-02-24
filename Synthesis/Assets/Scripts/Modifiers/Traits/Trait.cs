using System;
using Synthesis.Creatures.Visual;
using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
    [CreateAssetMenu(fileName = "Basic Trait", menuName = "ScriptableObjects/BasicTrait", order = 1)]
    // Container for Trait Strategies.
    public class Trait : ScriptableObject, ITrait, IModifier
    {

        private MoveType type = MoveType.Both;

        [Tooltip("{0} is additive value, {1} is multiplicative value, {2} is name.")]
        [SerializeField] [TextArea]
        protected string description = "This is a {2} trait that can add a flat value of {0} and a multiplier of x{1}.";
        [SerializeField] protected float additive = 0;
        [SerializeField] protected float multiplier = 1;
        [SerializeField] public CreaturePiece associatedPiece;
        [SerializeField] public Color color = Color.white;

        public MoveType Type { get => type; }
        public string Name { get => name; }

        public virtual string Description
        {
            get
            {
                return String.Format(description, additive, multiplier, name);
            }
        }

        public virtual void ApplyModifier(ref MoveInfo info)
        {
            info.attack.Additive += additive;
            info.attack.Multiplier *= multiplier;
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
