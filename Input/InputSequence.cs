using System.Collections.Generic;

namespace CAF.Input
{
    [System.Serializable]
    public class InputSequence
    {
        public int executeWindow = 3;
        public List<InputDefinition> executeInputs = new List<InputDefinition>();
        public int sequenceWindow = 8;
        public List<InputDefinition> sequenceInputs = new List<InputDefinition>();
    }
}