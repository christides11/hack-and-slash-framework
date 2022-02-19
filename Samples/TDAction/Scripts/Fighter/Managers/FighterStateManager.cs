using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterStateManager : MonoBehaviour, IFighterStateManager
    {
        public int CurrentState { get; } = 0;
        public int CurrentStateFrame { get; } = 0;
        
        public void AddState(StateTimeline state, int stateNumber)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveState(int stateNumber)
        {
            throw new System.NotImplementedException();
        }

        public bool ChangeState(int state, int stateFrame = 0, bool callOnInterrupt = true)
        {
            throw new System.NotImplementedException();
        }

        public StateTimeline GetState(int state)
        {
            throw new System.NotImplementedException();
        }

        public void SetFrame(int frame)
        {
            throw new System.NotImplementedException();
        }

        public void IncrementFrame()
        {
            throw new System.NotImplementedException();
        }
    }
}