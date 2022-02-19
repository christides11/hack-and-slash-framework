using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    public class FighterStateManager : MonoBehaviour, IFighterStateManager
    {
        public int CurrentState
        {
            get { return currentState; }
        }
        public int CurrentStateFrame
        {
            get { return currentStateFrame; }
        }

        protected Dictionary<int, StateTimeline> states = new Dictionary<int, StateTimeline>();
        public PlayableDirector director;

        private bool markedForStateChange = false;
        private int nextState = 0;
        public FighterManager fighterManager;

        public int currentState = 0;
        public int currentStateFrame = 0;
        
        private void Awake()
        {
            director.timeUpdateMode = DirectorUpdateMode.Manual;
        }

        public void Tick()
        {
            if (markedForStateChange)
            {
                ChangeState(nextState, 0, true);
            }
            if (CurrentState == 0) return;
            director.Evaluate();
        }
        
        public void AddState(HnSF.StateTimeline state, int stateNumber)
        {
            states.Add((int)stateNumber, (StateTimeline)state);
        }

        public void RemoveState(int stateNumber)
        {
            throw new System.NotImplementedException();
        }

        public void MarkForStateChange(int nextState)
        {
            markedForStateChange = true;
            this.nextState = nextState;
        }
        
        public bool ChangeState(int state, int stateFrame = 0, bool callOnInterrupt = true)
        {
            markedForStateChange = false;
            int oldState = CurrentState;
            int oldStateFrame = CurrentStateFrame;

            if(callOnInterrupt && oldState != 0)
            {
                SetFrame(states[CurrentState].totalFrames);
                director.Evaluate();
            }

            currentStateFrame = stateFrame;
            currentState = state;
            if(CurrentStateFrame == 0)
            {
                InitState();
                SetFrame(1);
            }

            return true;
        }
        
        public void InitState()
        {
            if (CurrentState == 0)
            {
                director.playableAsset = null;
                return;
            }
        
            director.playableAsset = states[CurrentState];
            foreach (var pAO in director.playableAsset.outputs)
            {
                director.SetGenericBinding(pAO.sourceObject, fighterManager);
            }
            director.Play();
            SetFrame(0);
            director.Evaluate();
        }

        public HnSF.StateTimeline GetState(int state)
        {
            return states[state];
        }

        public void SetFrame(int frame)
        {
            currentStateFrame = frame;
            director.time = (float)CurrentStateFrame * fighterManager.Manager.Simulation.TickRate;
        }

        public void IncrementFrame()
        {
            currentStateFrame++;
            director.time = (float)CurrentStateFrame * fighterManager.Manager.Simulation.TickRate;
        }
    }
}