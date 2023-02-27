namespace HnSF.Sample.TDAction.State
{
    public struct ConditionButton : IConditionVariables
    {
        public enum ButtonStateType
        {
            IsDown = 0,
            FirstPress = 1,
            Released = 2
        }
        
        public int FunctionMap => (int)ConditionFunctionEnum.BUTTON;
        
        public PlayerInputType button;
        public ButtonStateType buttonState;
        public int offset;
        public int buffer;

        public IConditionVariables Copy()
        {
            return new ConditionButton()
            {
                button = button,
                buttonState = buttonState,
                offset = offset,
                buffer = buffer
            };
        }
    }
}