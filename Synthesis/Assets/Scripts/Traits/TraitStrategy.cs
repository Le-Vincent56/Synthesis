using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Traits
{
    // Class that handles the trait logic and checking.
    [Serializable]
    public class TraitStrategy
    {
        [SerializeField] protected float additive;
        [SerializeField] protected float multiplier;
        
        public virtual void Activate(ref MoveInfo info)
        {
            info.Additive += additive;
            info.Multiplier += multiplier;
        }
    }
}
