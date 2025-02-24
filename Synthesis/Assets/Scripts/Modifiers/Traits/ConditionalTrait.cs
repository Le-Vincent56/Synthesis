using UnityEngine;

namespace Synthesis.Modifiers.Traits
{
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

        public override void ApplyModifier(ref MoveInfo info)
        {
            // current weather, which we get somehow from somewhere.
            if (weather == TempWeather.Rain)
            {
                base.ApplyModifier(ref info);
            }
        }
    }
}
