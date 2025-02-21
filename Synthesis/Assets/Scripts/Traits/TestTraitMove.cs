using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Traits
{
    public class TestTraitMove : MonoBehaviour
    {
        public TraitsNavigator navigator = new TraitsNavigator(MoveType.Both);
        [SerializeField] private int numTraits = 4;
        
        // Start is called before the first frame update
        void Start()
        {
            var info = new MoveInfo(10, MoveType.Attack);

            for (int i = 0; i < numTraits; i++)
            {
                navigator.AddTrait(TraitPoolManager.Instance.traitPool.GetRandomTrait());
            }
            
            navigator.ActivateStrategy(ref info);
            
            Debug.Log(info.FinalValue);
        }
    }
}
