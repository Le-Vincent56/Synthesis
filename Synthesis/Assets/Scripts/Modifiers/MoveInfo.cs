namespace Synthesis.Modifiers
{
    // Used to pass data across traits.
    public struct MoveInfo
    {
        public MoveInfo(float baseValue, MoveType moveType)
        {
            this.baseValue = baseValue;
            finalValue = this.baseValue;
            type = moveType;
            additive = 0;
            multiplier = 1;
        }
        
        private MoveType type;
        private float baseValue;
        private float additive;
        private float multiplier;
        private float finalValue;
        
        public MoveType Type
        {
            get => type;
            private set => type = value;
        }

        public float BaseValue
        {
            get => baseValue;
            private set => baseValue = value;
        }

        public float FinalValue
        {
            get => finalValue;
            set => finalValue = value;
        }

        public float Additive
        {
            get => additive;
            set => additive = value;
        }
        
        public float Multiplier
        {
            get => multiplier;
            set => multiplier = value;
        }
    }

    
    // Used to identify what a trait belongs to.
    public enum MoveType
    {
        Attack,
        Synthesize,
        Both
    }
}
