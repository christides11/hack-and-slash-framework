using HnSF.Combat;

namespace HnSF.Fighters
{
    public interface IFighterStateManager
    {
        int MovesetCount { get; }
        int CurrentStateMoveset { get; }
        int CurrentState { get; }
        int CurrentStateFrame { get; }
        
        
        IMovesetDefinition GetMoveset(int index);
        void MarkForStateChange(int state, int moveset = -1, int frame = 0, bool force = false);
        bool ChangeState(int state, int moveset = -1, int stateFrame = 0, bool callOnInterrupt = true);
        StateTimeline GetState(int state);
        StateTimeline GetState(int moveset, int state);
        void SetMoveset(int index);
        void SetFrame(int frame);
        void IncrementFrame(int amt = 1);
    }
}