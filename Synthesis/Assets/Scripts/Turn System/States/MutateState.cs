using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class MutateState : TurnSubState
    {
        public MutateState() { }

        public override void OnEnter()
        {
            Debug.Log("Entered Mutate State");
        }
    }
}
