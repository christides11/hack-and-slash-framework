namespace HnSF.Sample.TDAction.State
{
    public struct ConditionFallSpeed : IConditionVariables
    {
        public int FunctionMap => (int)ConditionFunctionEnum.FALL_SPEED;

        public float minValue;
        public float maxValue;
        public bool inverse;
    }
}