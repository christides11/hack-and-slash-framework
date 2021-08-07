using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HnSF.Input
{
    [System.Serializable]
    public struct InputRecordButton : InputRecordInput
    {
        public bool isDown;
        public bool firstPress;
        public bool released;

        public InputRecordButton(bool button)
        {
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
    }
}