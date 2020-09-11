namespace CAF.Input
{
    public interface InputRecordInput
    {
        bool UsedInBuffer();
        void UseInBuffer();
        void Process(InputRecordInput lastStateDown);
    }
}