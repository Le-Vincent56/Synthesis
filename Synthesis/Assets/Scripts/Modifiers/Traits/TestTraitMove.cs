using Synthesis.Creatures;
using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
    public class TestTraitMove : MonoBehaviour
    {
        public TraitsNavigator navigator = new TraitsNavigator(MoveType.Both);
        public Creature creature;
        [SerializeField] private int numTraits = 4;
        
        // Start is called before the first frame update
        void Start()
        {
            var info = new MoveInfo(10, MoveType.Attack);

            for (int i = 0; i < numTraits; i++)
            {
                AddRandomTrait();
            }

            navigator.ActivateStrategy(ref info);
            
            Debug.Log(info.FinalValue);
        }

        private void AddRandomTrait()
        {
            var trait = TraitPoolManager.Instance.traitPool.GetRandomTrait();
            AddTrait(trait);
        }

        private void AddTrait(Trait trait)
        {
            if (navigator.AddTrait(trait))
            {
                foreach (var connector in creature.piece.connectors)
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
