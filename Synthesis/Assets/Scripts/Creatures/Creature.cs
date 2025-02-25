using System.Collections;
using System.Collections.Generic;
using Synthesis.Creatures.Visual;
using Synthesis.Modifiers;
using Synthesis.Modifiers.Traits;
using Unity.VisualScripting;
using UnityEngine;

namespace Synthesis.Creatures
{
    public class Creature : MonoBehaviour
    {
        public float hp,strength,attack1damage, attack2damage, strengthincrease1, strengthincrease2;
        public string attack1, attack2, strength1, strength2;

        public TraitsNavigator[] moves = new TraitsNavigator[2];

        public CreaturePiece piece;
        
        // Methods
        /// <summary>
        /// Adds a random trait to a move of the creature.
        /// </summary>
        /// <param name="index"></param>
        private void AddRandomTrait(int index)
        {
            var trait = TraitPoolManager.Instance.traitPool.GetRandomTrait();
            AddTrait(trait, index);
        }

        /// <summary>
        /// Adds a new trait to a move of the creature
        /// </summary>
        /// <param name="trait"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool AddTrait(Trait trait, int index)
        {
            if (index >= moves.Length || index < 0) return false;
            
            if (moves[index].AddTrait(trait))
            {
                UpdateCreatureVisual(trait);

                return true;
            }

            return false;
        }
        
        public bool AddTrait(Trait trait)
        {
            if (trait.Type == MoveType.Attack || trait.Type == MoveType.Both)
            {
                return AddTrait(trait, 0);
            }
            else
            {
                return AddTrait(trait, 1);
            }
        }

        /// <summary>
        /// Updates creature based on trait added.
        /// </summary>
        /// <param name="trait"></param>
        private void UpdateCreatureVisual(Trait trait)
        {
            foreach (var connector in piece.connectors)
            {
                var con = connector;
                var oldPiece = piece;
                while (con.child)
                {
                    oldPiece = con.child;
                    con = con.child.connectors[0];
                }
                var newPiece = Instantiate(trait.associatedPiece);
                newPiece.transform.position = con.transform.position;
                newPiece.transform.parent = con.transform;
                if (oldPiece.primaryColorIn != null || oldPiece.primaryColorIn[0] != null)
                {
                    newPiece.SetPartColor(Color.Lerp(trait.color, oldPiece.primaryColorIn[0].color, 0.4f));
                }
                else
                {
                    newPiece.SetPartColor(trait.color);
                }
                con.child = newPiece;
            }
        }
    }
    
}
