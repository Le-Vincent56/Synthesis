using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
    /// <summary>
    /// Stores a list of traits and applies them to a move.
    /// </summary>
    [Serializable] public class TraitsNavigator
    {
        [SerializeField] private List<Trait> traits = new List<Trait>();
        [SerializeField] private MoveType type;

        public TraitsNavigator(MoveType type)
        {
            this.type = type;
        }

        public List<Trait> Traits => traits;
        public MoveType Type => type;

        /// <summary>
        /// Applies the traits to a move.
        /// </summary>
        /// <param name="info"></param>
        public void ActivateStrategy(ref MoveInfo info)
        {
            // Iterate through each strategy and apply modifiers.
            foreach (var trait in traits)
            {
                trait.ApplyModifier(ref info);
            }
            
            // Apply final calculation logic.
            info.FinalValue = (info.BaseValue + info.Additive) * info.Multiplier;
        }

        /// <summary>
        /// Adds a trait to the trait navigator
        /// </summary>
        /// <param name="trait"></param>
        public bool AddTrait(Trait trait)
        {
            // Both on either means this check doesn't need to happen
            if (type == MoveType.Both || trait.Type == MoveType.Both)
            {
                traits.Add(trait);
                return true;
            }
            // Add if traits are the same
            else if(type == trait.Type)
            {
                traits.Add(trait);
                return true;
            }
            else
            {
                Debug.LogError($"Attempted to add trait of incorrect type. Trait '{trait.Name}' is of " +
                               $"type {trait.Type} but TraitNavigator is for {type} traits.");
                return false;
            }
        }
    }
}
