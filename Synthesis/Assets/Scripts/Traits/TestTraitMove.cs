using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Traits
{
    public class TestTraitMove : MonoBehaviour
    {
        public TraitsNavigator navigator = new TraitsNavigator(MoveType.Both);
        
        // Start is called before the first frame update
        void Start()
        {
            var info = new MoveInfo(10, MoveType.Attack);
            
            navigator.ActivateStrategy(ref info);
            
            Debug.Log(info.FinalValue);
        }
    }
}
