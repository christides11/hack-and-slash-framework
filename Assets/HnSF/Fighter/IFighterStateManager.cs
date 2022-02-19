namespace HnSF.Fighters
{
    public interface IFighterStateManager
    {
        int CurrentState { get; }
        int CurrentStateFrame { get; }

        void AddState(StateTimeline state, int stateNumber);
        void RemoveState(int stateNumber);
        bool ChangeState(int state, int stateFrame = 0, bool callOnInterrupt = true);
        StateTimeline GetState(int state);
        void SetFrame(int frame);
        void IncrementFrame();
    }
}