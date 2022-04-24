using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Combat;
using HnSF.Fighters;
using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    public class FighterStateManager : MonoBehaviour, IFighterStateManager
    {
        public int MovesetCount
        {
            get { return fighterManager.definition.movesets.Length; }
        }
        [field: SerializeField]
        public int CurrentStateMoveset { get; protected set; } = 0;
        [field: SerializeField]
        public int CurrentState { get; protected set; } = 0;
        [field: SerializeField]
        public int CurrentStateFrame { get; protected set; } = 0;

        protected Dictionary<int, StateTimeline> states = new Dictionary<int, StateTimeline>();

        private bool markedForStateChange = false;
        private int nextState = 0;
        public FighterManager fighterManager;

        private void Awake()
        {
            
        }

        public void Tick()
        {
            if (markedForStateChange)
            {
                ChangeState(nextState, 0, true);
            }
            if (CurrentState == 0) return;
        }

        public void MarkForStateChange(int nextState)
        {
            markedForStateChange = true;
            this.nextState = nextState;
        }

        public Combat.MovesetDefinition GetMoveset(int index)
        {
            throw new NotImplementedException();
        }

        public bool ChangeState(int state, int stateFrame = 0, bool callOnInterrupt = true)
        {
            markedForStateChange = false;
            int oldState = CurrentState;
            int oldStateFrame = CurrentStateFrame;

            if(callOnInterrupt && oldState != 0)
            {
                SetFrame(states[CurrentState].totalFrames);
            }

            CurrentStateFrame = stateFrame;
            CurrentState = state;
            if(CurrentStateFrame == 0)
            {
                InitState();
                SetFrame(1);
            }

            return true;
        }
        
        public void InitState()
        {
            if (CurrentState == 0) return;
            SetFrame(0);
        }

        public HnSF.StateTimeline GetState(int state)
        {
            return states[state];
        }

        public HnSF.StateTimeline GetState(int moveset, int state)
        {
            throw new NotImplementedException();
        }

        public void SetMoveset(int movesetIndex)
        {
            throw new NotImplementedException();
        }

        public void SetFrame(int frame)
        {
            CurrentStateFrame = frame;
        }

        public void IncrementFrame()
        {
            CurrentStateFrame++;
        }
    }
}