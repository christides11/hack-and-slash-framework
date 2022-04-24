﻿namespace HnSF.Fighters
{
    public interface IFighterStateManager
    {
        int MovesetCount { get; }
        int CurrentStateMoveset { get; }
        int CurrentState { get; }
        int CurrentStateFrame { get; }
        
        
        Combat.MovesetDefinition GetMoveset(int index);
        void MarkForStateChange(int state);
        bool ChangeState(int state, int stateFrame = 0, bool callOnInterrupt = true);
        StateTimeline GetState(int state);
        StateTimeline GetState(int moveset, int state);
        void SetMoveset(int movesetIndex);
        void SetFrame(int frame);
        void IncrementFrame();
    }
}