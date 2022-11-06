namespace HnSF.Combat
{
    public interface IMovesetDefinition
    {
        public StateTimeline GetState(int stateID);
    }
}