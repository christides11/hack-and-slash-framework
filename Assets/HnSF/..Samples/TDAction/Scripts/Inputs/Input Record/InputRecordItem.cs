using System.Collections.Generic;

namespace HnSF.Input
{
    public class InputRecordItem
    {
        public Dictionary<int, InputRecordInput> inputs;

        public InputRecordItem()
        {
            inputs = new Dictionary<int, InputRecordInput>();
        }

        public virtual bool AddInput(int inputID, InputRecordInput input)
        {
            if (inputs.ContainsKey(inputID))
            {
                return false;
            }
            inputs.Add(inputID, input);
            return true;
        }
    }
}