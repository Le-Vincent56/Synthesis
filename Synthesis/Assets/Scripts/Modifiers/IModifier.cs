namespace Synthesis.Modifiers
{
    public interface IModifier
    {

        void ApplyModifier(ref MoveInfo moveInfo);
    }
}
