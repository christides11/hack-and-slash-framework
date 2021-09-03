namespace HnSF.Input
{
    public interface InputRecordInput
    {
        void Process(InputRecordInput lastStateDown);
    }
}