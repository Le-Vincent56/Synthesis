using Synthesis.Creatures.Visual;
using Synthesis.Modifiers;
using Synthesis.Modifiers.Traits;
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
        public Move Infect { get => infect; }
        public Move Synthesize { get => synthesize; }

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
        private bool AddTrait(Trait trait, MoveType moveType)
        {
            return moveType switch
            {
                MoveType.Attack => infect.AddTrait(trait),
                MoveType.Synthesize => synthesize.AddTrait(trait),
                MoveType.Both => infect.AddTrait(trait) && synthesize.AddTrait(trait),
                _ => false
            };
        }

        public bool AddTrait(Trait trait)
        {
            if (trait.Type == MoveType.Attack || trait.Type == MoveType.Both)
            {
                return AddTrait(trait, MoveType.Attack);
            }
            else
            {
                return AddTrait(trait, MoveType.Synthesize);
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

        /// <summary>
        /// Calculate the points
        /// </summary>
        public float CalculatePoints()
        {
            moveInfo = new MoveInfo(MoveType.Attack, 10);

            infect.ActivateStrategy(ref moveInfo);

            return moveInfo.attack.FinalValue;
        }
    }
}
