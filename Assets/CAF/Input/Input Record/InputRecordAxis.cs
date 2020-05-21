namespace CAF.Input
{
    [System.Serializable]
    public struct InputRecordAxis : InputRecordInput
    {
        public float axis;

        public InputRecordAxis(float axis)
        {
            this.axis = axis;
        }

        public bool UsedInBuffer()
        {
            return false;
        }

        public void Process(InputRecordInput lastStateDown)
        {

        }
    }
}