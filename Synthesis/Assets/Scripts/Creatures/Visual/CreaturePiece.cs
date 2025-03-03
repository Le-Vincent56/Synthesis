// by Ryan Zhang from GMTK Game Jam 2024

using UnityEngine;

namespace Synthesis.Creatures.Visual
{
    public class CreaturePiece : MonoBehaviour
    {
        [SerializeField] public CreaturePieceConnector[] connectors;
        [SerializeField] private Vector3 scaleMod;

        [SerializeField] public SpriteRenderer[] primaryColorIn;
        [SerializeField] public SpriteRenderer[] tertiaryColorIn;
        
        private static readonly int Color0 = Shader.PropertyToID("_Color0");
        private static readonly int Color1 = Shader.PropertyToID("_Color1");

        // Change the size of current part.
        public void SetPartSize(float height, float width)
        {
            var p = transform.parent;

            transform.parent = null;
        
            foreach (var connector in connectors)
            {
                if (connector.child != null)
                {
                    connector.child.transform.parent = null;
                }
            }

            transform.localScale = new Vector3(width * scaleMod.x, height * scaleMod.y, 1);
        
            foreach (var connector in connectors)
            {
                if (connector.child != null)
                {
                    connector.child.transform.parent = connector.transform;
                    connector.child.transform.position = connector.transform.position;
                }
            }

            transform.parent = p;
        }

        public void SetPartColor(Color color, ColorElement element = ColorElement.All)
        {
            switch (element)
            {
                case ColorElement.Primary:
                    foreach (var spr in primaryColorIn)
                    {
                        //spr.color = color;
                        spr.material.SetColor(Color0, color);
                    }
                    break;
                case ColorElement.Secondary:
                    foreach (var spr in primaryColorIn)
                    {
                        //spr.color = color;
                        spr.material.SetColor(Color1, color);
                    }
                    break;
                case ColorElement.Tertiary:
                    foreach (var spr in tertiaryColorIn)
                    {
                        spr.color = color;
                    }
                    break;
                case ColorElement.All:
                    foreach (var spr in primaryColorIn)
                    {
                        spr.color = color;
                    }
                    foreach (var spr in primaryColorIn)
                    {
                        spr.color = color;
                    }
                    foreach (var spr in tertiaryColorIn)
                    {
                        spr.color = color;
                    }
                    break;
            }
        }
    
        void OnDrawGizmos()
        {
            // Draws the Light bulb icon at position of the object.
            // Because we draw it inside OnDrawGizmos the icon is also pickable
            // in the scene view.

            Gizmos.DrawIcon(transform.position, "Mark.png", false);
        }
    }

    public enum ColorElement
    {
        Primary,
        Secondary,
        Tertiary,
        All
    }
}