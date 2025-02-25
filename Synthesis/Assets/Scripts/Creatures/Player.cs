using Synthesis.Creatures.Visual;
using Synthesis.Modifiers;
using Synthesis.Modifiers.Mutations;
using UnityEngine;

namespace Synthesis.Creatures
{
    public class Player : MonoBehaviour, ICreature
    {
        private CreaturePiece piece;
        private Move infect = new Move(MoveType.Attack);
        private Move synthesize = new Move(MoveType.Synthesize);

        private MoveInfo moveInfo;

        public CreaturePiece Piece { get => piece; }

        // Methods
        /// <summary>
        /// Adds a random trait to a move of the creature.
        /// </summary>
        private void AddRandomTrait(MoveType moveType)
        {
            var trait = TraitPoolManager.Instance.traitPool.GetRandomTrait();
            AddTrait(trait, moveType);
        }

        /// <summary>
        /// Adds a new trait to a move of the creature
        /// </summary>
        private bool AddTrait(Mutation trait, MoveType moveType)
        {
            return moveType switch
            {
                MoveType.Attack => infect.AddTrait(trait),
                MoveType.Synthesize => synthesize.AddTrait(trait),
                MoveType.Both => infect.AddTrait(trait) && synthesize.AddTrait(trait),
                _ => false
            };
        }

        /// <summary>
        /// Updates creature based on trait added.
        /// </summary>
        private void UpdateCreatureVisual(Mutation trait)
        {
            foreach (CreaturePieceConnector connector in piece.connectors)
            {
                CreaturePieceConnector con = connector;
                while (con.child)
                {
                    con = con.child.connectors[0];
                }
                CreaturePiece piece = Instantiate(trait.associatedPiece);
                piece.transform.position = con.transform.position;
                piece.transform.parent = con.transform;
                piece.SetPartColor(trait.color);
                con.child = piece;
            }
        }

        public void CalculatePoints()
        {
            moveInfo = new MoveInfo(MoveType.Attack, 10);


        }
    }
}
