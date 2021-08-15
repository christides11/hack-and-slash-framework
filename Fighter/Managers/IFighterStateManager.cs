using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public interface IFighterStateManager
    {
        ushort CurrentState { get; }
        uint CurrentStateFrame { get; }

        void Tick();
        void LateTick();
        void AddState(FighterStateBase state, ushort stateNumber);
        bool ChangeState(ushort state, uint stateFrame = 0, bool callOnInterrupt = true);
        FighterStateBase GetState(ushort state);
        void SetFrame(uint frame);
        void IncrementFrame();
    }
}