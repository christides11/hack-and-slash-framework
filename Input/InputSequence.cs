using System.Collections.Generic;

namespace CAF.Input
{
    [System.Serializable]
    public class InputSequence
    {
        public List<InputDefinition> executeInputs = new List<InputDefinition>();
        public List<InputDefinition> sequenceInputs = new List<InputDefinition>();
    }
}