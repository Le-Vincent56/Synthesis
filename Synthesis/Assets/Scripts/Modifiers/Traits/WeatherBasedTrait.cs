using System;
using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
    /// <summary>
    /// Trait that checks for a weather conditional.
    /// </summary>
    public class WeatherBasedTrait : Trait
    {
        private enum TempWeather
        {
            Rain,
            Sun,
            Snow,
            Sandstorm,
            GaleWind,
        }

        [SerializeField] private TempWeather weather;
        [SerializeField] private bool not;
        [SerializeField] [TextArea] protected string weatherConditional = "While {1}in {2}, {0}";

        public override string Description
        {
            get
            {
                var notString = not ? "not " : "";
                var weatherString = "Rain";
                return String.Format(weatherConditional, base.Description, notString, weatherString);
            }
        }

        public override void ApplyModifier(ref MoveInfo info)
        {
            // current weather, which we get somehow from somewhere.
            if (not ^ weather == TempWeather.Rain)
            {
                base.ApplyModifier(ref info);
            }
        }
    }
}
