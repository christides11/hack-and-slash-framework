namespace HnSF.Fighters
{
    public interface IFighterStateManager
    {
        ushort CurrentState { get; }
        uint CurrentStateFrame { get; }

        void AddState(FighterStateBase state, ushort stateNumber);
        void RemoveState(ushort stateNumber);
        bool ChangeState(ushort state, uint stateFrame = 0, bool callOnInterrupt = true);
        FighterStateBase GetState(ushort state);
        void SetFrame(uint frame);
        void IncrementFrame();
    }
}