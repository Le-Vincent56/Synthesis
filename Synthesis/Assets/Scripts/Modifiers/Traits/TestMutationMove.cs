using Synthesis.Creatures;
using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
    public class TestMutationMove : MonoBehaviour
    {
        public Move navigator = new Move(MoveType.Both);
        public Player player;
        [SerializeField] private int numTraits = 4;
        
        // Start is called before the first frame update
        void Start()
        {
            var info = new MoveInfo(MoveType.Attack, attackValue: 10);

            for (int i = 0; i < numTraits; i++)
            {
                AddRandomTrait();
            }

            navigator.ActivateStrategy(ref info);
            
            Debug.Log(info.attack.FinalValue);
        }

        private void AddRandomTrait()
        {
            var trait = TraitPoolManager.Instance.traitPool.GetRandomTrait();
            AddTrait(trait);
        }

        private void AddTrait(Trait trait)
        {
            if (!navigator.AddTrait(trait)) return;

            // Exit case - there's no player
            if (!player) return;

            if (player)
            {
                foreach (var connector in player.Piece.connectors)
                {
                    var con = connector;
                    while (con.child)
                    {
                        con = con.child.connectors[0];
                    }
                    var piece = Instantiate(trait.associatedPiece);
                    piece.transform.position = con.transform.position;
                    piece.transform.parent = con.transform;
                    piece.SetPartColor(trait.color);
                    con.child = piece;
                }
            }
        }
    }
}
