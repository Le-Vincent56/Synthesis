// by Ryan Zhang from GMTK Game Jam 2024

using System;
using Synthesis.EventBus;
using Synthesis.EventBus.Events.UI;
using Synthesis.Mutations;
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

        private MutationStrategy associatedMutation;
        private Vector3 defaultScale;
        private bool highlighted;

        private EventBinding<HighlightMutation> onHighlightMutation;

        private void Start()
        {
            defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            onHighlightMutation = new EventBinding<HighlightMutation>(HighlightPiece);
            EventBus<HighlightMutation>.Register(onHighlightMutation);
        }

        private void OnDisable()
        {
            EventBus<HighlightMutation>.Deregister(onHighlightMutation);
        }

        public void HighlightPiece(HighlightMutation eventData)
        {
            if (associatedMutation != null)
            {
                if (eventData.mutation == associatedMutation)
                {
                    // Do stuff
                    SetPartSize(false);
                    SetPartColor(associatedMutation.Color1 + associatedMutation.Color1, ColorElement.Secondary);
                    highlighted = true;
                }
                else if (highlighted)
                {
                    SetPartSize(true);
                    SetPartColor(associatedMutation.Color1, ColorElement.Secondary);
                    highlighted = false;
                }
            }
        }

        public void SetMutation(MutationStrategy mutation, Color bleed, float bleedRate)
        {
            associatedMutation = mutation;
            
            SetPartColor(Color.Lerp(mutation.Color0, bleed, bleedRate), ColorElement.Primary);
            SetPartColor(mutation.Color0, ColorElement.Primary);
            SetPartColor(mutation.Color1, ColorElement.Secondary);
            
            SetPartColor(mutation.Color2, ColorElement.Tertiary);

            defaultScale = transform.localScale;
        }

        // Change the size of current part.
        public void SetPartSize(bool reset)
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

            if (reset)
            {
                transform.localScale = defaultScale;
            }
            else
            {
                transform.localScale = new Vector3(defaultScale.x * scaleMod.x, defaultScale.y * scaleMod.y, 1);
            }
            
        
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
                    if (tertiaryColorIn.Length > 0 && tertiaryColorIn[0] != null)
                    {
                        foreach (var spr in tertiaryColorIn)
                        {
                            spr.color = color;
                        }
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

                    if (tertiaryColorIn.Length > 0 && tertiaryColorIn[0] != null)
                    {
                        foreach (var spr in tertiaryColorIn)
                        {
                            spr.color = color;
                        }
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