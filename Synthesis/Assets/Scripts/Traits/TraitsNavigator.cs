using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Traits
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
            foreach (var trait in traits)
            {
                trait.Activate(ref info);
            }
        }

        /// <summary>
        /// Adds a trait to the trait navigator
        /// </summary>
        /// <param name="trait"></param>
        public void AddTrait(Trait trait)
        {
            // Both on either means this check doesn't need to happen
            if (type == MoveType.Both || trait.Type == MoveType.Both)
            {
                traits.Add(trait);
            }
            // Add if traits are the same
            else if(type == trait.Type)
            {
                traits.Add(trait);
            }
            else
            {
                Debug.LogError($"Attempted to add trait of incorrect type. Trait '{trait.Name}' is of " +
                               $"type {trait.Type} but TraitNavigator is for {type} traits.");
            }
        }
    }
}
