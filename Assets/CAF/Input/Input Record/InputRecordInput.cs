namespace CAF.Input
{
    public interface InputRecordInput
    {
        void Process(InputRecordInput lastStateDown);
    }
}