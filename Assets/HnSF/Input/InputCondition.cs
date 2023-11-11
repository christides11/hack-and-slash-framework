namespace HnSF.Input
{
    [System.Serializable]
    public class InputCondition
    {
        public InputBitmask[] sequence;
        public int impreciseInputCount;
        public bool inputAllowDisable;
        public EnterInputMethod method;
    }
}