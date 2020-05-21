namespace CAF.Input
{
    public interface InputRecordInput
    {
        bool UsedInBuffer();
        void Process(InputRecordInput lastStateDown);
    }
}