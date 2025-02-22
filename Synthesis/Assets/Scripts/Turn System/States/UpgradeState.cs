using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.Turns.States
{
    public class UpgradeState : TurnState
    {
        public UpgradeState(TurnSystem turnSystem) : base(turnSystem)
        {

        }

        public override void OnEnter()
        {
            Debug.Log("Upgrading to gain a trait");
        }
    }
}
