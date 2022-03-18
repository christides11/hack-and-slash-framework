namespace HnSF.Sample.TDAction
{
    public struct InputButtonData
    {
        public bool isDown;
        public bool firstPress;
        public bool released;
        
        public InputButtonData(bool value, InputButtonData previousFrame)
        {
            isDown = value;
            firstPress = value == true && (previousFrame.isDown == false) ? true : false;
            released = value == false && previousFrame.isDown == true ? true : false;
        }
    }
}