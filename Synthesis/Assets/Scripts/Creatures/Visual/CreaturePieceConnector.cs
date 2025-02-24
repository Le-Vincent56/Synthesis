// by Ryan Zhang from GTMK Game Jam 2024

using System;
using UnityEngine;

namespace Synthesis.Creatures.Visual
{
    [Serializable] public class CreaturePieceConnector
    {
        [SerializeField] public Transform transform;
        [SerializeField] public CreaturePiece child;
    }
}