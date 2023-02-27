namespace HnSF
{
    public interface IConditionVariables
    {
        public int FunctionMap { get; }

        IConditionVariables Copy();
    }
}