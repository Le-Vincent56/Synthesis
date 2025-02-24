using System.Collections;
using System.Collections.Generic;
using Synthesis.Creatures;
using UnityEngine;

namespace Synthesis.Traits
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
                var trait = TraitPoolManager.Instance.traitPool.GetRandomTrait();
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

            navigator.ActivateStrategy(ref info);
            
            Debug.Log(info.FinalValue);
        }
    }
}
