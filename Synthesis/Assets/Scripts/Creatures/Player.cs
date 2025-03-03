using System;
using Synthesis.Creatures.Visual;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.Turns;
using Synthesis.Modifiers;
using Synthesis.Modifiers.Traits;
using Synthesis.Mutations;
using UnityEngine;

namespace Synthesis.Creatures
{
    public enum MutationPartType
    {
        Infect = 0,
        Synthesis = 1,
        InfectDrought = 2,
        InfectTorrent = 4,
        SynthesisDrought = 3,
        SynthesisTorrent = 5,
        // TODO: Make actual weather variations
    }
    
    public class Player : MonoBehaviour, ICreature
    {
        [SerializeField] private CreaturePiece[] availablePieces;
        
        [SerializeField] private CreaturePiece piece;
        private Move infect = new Move(MoveType.Attack);
        private Move synthesize = new Move(MoveType.Synthesize);

        private MoveInfo moveInfo;
        private static readonly int Color1 = Shader.PropertyToID("_Color1");

        private EventBinding<Synthesize> onSynthesize;

        public CreaturePiece Piece { get => piece; }
        public Move Infect { get => infect; }
        public Move Synthesize { get => synthesize; }

        // Methods
        private void OnEnable()
        {
            onSynthesize = new EventBinding<Synthesize>(OnSynthesize);
            EventBus<Synthesize>.Register(onSynthesize);
        }

        private void OnDisable()
        {
            EventBus<Synthesize>.Deregister(onSynthesize);
        }

        private void OnSynthesize(Synthesize eventData)
        {
            UpdateCreatureVisual(eventData.Mutation);
        }
        
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
            UpdateCreatureVisual(trait);
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
                newPiece.transform.position = (con.transform.position + new Vector3(0, 0, 0.01f));
                newPiece.transform.parent = con.transform;
                if (oldPiece.primaryColorIn != null && newPiece.primaryColorIn.Length > 0 && oldPiece.primaryColorIn[0] != null)
                {
                    newPiece.SetPartColor(Color.Lerp(trait.color, oldPiece.primaryColorIn[0].material.GetColor(Color1), 0.4f), ColorElement.Primary);
                    newPiece.SetPartColor(trait.color1, ColorElement.Secondary);
                }
                else
                {
                    newPiece.SetPartColor(trait.color, ColorElement.Primary);
                    newPiece.SetPartColor(trait.color1, ColorElement.Secondary);
                }
                
                if (newPiece.tertiaryColorIn != null && newPiece.tertiaryColorIn.Length > 0 && newPiece.tertiaryColorIn[0] != null)
                {
                    newPiece.SetPartColor(trait.colorPattern, ColorElement.Tertiary);
                }
                
                con.child = newPiece;
            }
        }
        
        /// <summary>
        /// Updates creature based on mutation added.
        /// </summary>
        /// <param name="trait"></param>
        private void UpdateCreatureVisual(MutationStrategy mutation)
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
                var newPiece = Instantiate(availablePieces[(int)mutation.PartType]);
                newPiece.transform.position = (con.transform.position + new Vector3(0, 0, 0.01f));
                newPiece.transform.parent = con.transform;
                if (oldPiece.primaryColorIn != null && newPiece.primaryColorIn.Length > 0 && oldPiece.primaryColorIn[0] != null)
                {
                    newPiece.SetPartColor(Color.Lerp(mutation.Color0, oldPiece.primaryColorIn[0].material.GetColor(Color1), 0.4f), ColorElement.Primary);
                    newPiece.SetPartColor(mutation.Color1, ColorElement.Secondary);
                }
                else
                {
                    newPiece.SetPartColor(mutation.Color0, ColorElement.Primary);
                    newPiece.SetPartColor(mutation.Color1, ColorElement.Secondary);
                }
                
                if (newPiece.tertiaryColorIn != null && newPiece.tertiaryColorIn.Length > 0 && newPiece.tertiaryColorIn[0] != null)
                {
                    newPiece.SetPartColor(mutation.Color2, ColorElement.Tertiary);
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
