using UnityEngine;

namespace HnSF.Input
{
    [System.Serializable]
    public struct InputRecordAxis2D : InputRecordInput
    {
        public Vector2 axis2D;

        public InputRecordAxis2D(Vector2 axis2D)
        {
            this.axis2D = axis2D;
        }

        public bool UsedInBuffer()
        {
            return false;
        }

        public void UseInBuffer()
        {

        }

        public void Process(InputRecordInput lastStateDown)
        {
        }
    }
}