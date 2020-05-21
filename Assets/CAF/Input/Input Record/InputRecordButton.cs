namespace CAF.Input
{
    [System.Serializable]
    public struct InputRecordButton : InputRecordInput
    {
        public bool usedInBuffer;
        public bool isDown;
        public bool firstPress; //If the button was pressed on this frame
        public bool released; //If the button was released this frame

        public InputRecordButton(bool button)
        {
            usedInBuffer = false;
            isDown = button;
            firstPress = false;
            released = false;
        }

        public void Process(InputRecordInput lastInput)
        {
            InputRecordButton lsb = (InputRecordButton)lastInput;
            if (isDown && !lsb.isDown)
            {
                firstPress = true;
            }
            else if (!isDown && lsb.isDown)
            {
                released = true;
            }
        }

        public bool UsedInBuffer()
        {
            return usedInBuffer;
        }

        public void UseInBuffer()
        {
            usedInBuffer = true;
        }
    }
}