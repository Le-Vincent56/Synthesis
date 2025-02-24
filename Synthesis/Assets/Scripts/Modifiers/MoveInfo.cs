namespace Synthesis.Modifiers
{
    // Used to pass data across traits.
    public struct MoveAttribute
    {
        public MoveAttribute(float baseValue)
        {
            this.baseValue = baseValue;
            finalValue = baseValue;
            additive = 0;
            multiplier = 1;
        }
        
        private float baseValue;
        private float finalValue;
        private float additive;
        private float multiplier;

        public float BaseValue
        {
            get => baseValue;
            set => baseValue = value;
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

    public struct MoveInfo
    {
        public MoveInfo(MoveType moveType, float attackValue = 0, float mutationValue = 0, float healValue = 0)
        {
            type = moveType;
            attack = new MoveAttribute(attackValue);
            mutate = new MoveAttribute(mutationValue);
            heal = new MoveAttribute(healValue);
        }
        
        private MoveType type;
        public MoveType Type
        {
            get => type;
            private set => type = value;
        }

        public MoveAttribute attack;
        public MoveAttribute mutate;
        public MoveAttribute heal;
        
    }

    
    // Used to identify what a trait belongs to.
    public enum MoveType
    {
        Attack,
        Synthesize,
        Both
    }
    
    // Used to change where values are added
    public enum ModifierTarget
    {
        Attack,
        Mutate,
        Heal,
        HealMutate,
    }
}
