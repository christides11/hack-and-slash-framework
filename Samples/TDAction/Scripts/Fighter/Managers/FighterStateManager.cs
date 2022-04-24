using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Combat;
using HnSF.Fighters;
using HnSF.Sample.TDAction.State;
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

        [NonSerialized] public StateFunctionMapper functionMapper = new StateFunctionMapper(); 
        [NonSerialized] public StateConditionMapper conditionMapper = new StateConditionMapper();

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
        
        private void ProcessState()
        {
            for (int i = 0; i < states[CurrentState].data.Length; i++)
            {
                var valid = true;
                for (int j = 0; j < states[CurrentState].data[i].FrameRanges.Length; j++)
                {
                    if (CurrentStateFrame < states[CurrentState].data[i].FrameRanges[j].x
                        || CurrentStateFrame > states[CurrentState].data[i].FrameRanges[j].y)
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid) continue;
                if (!conditionMapper.TryCondition(states[CurrentState].data[i].Condition.FunctionMap, fighterManager, states[CurrentState].data[i].Condition)) continue;
                functionMapper.functions[states[CurrentState].data[i].FunctionMap](fighterManager, states[CurrentState].data[i]);
            }

            if (states[CurrentState].autoIncrement)
            {
                IncrementFrame(1);
                if (states[CurrentState].autoLoop && CurrentStateFrame > states[CurrentState].totalFrames)
                {
                    SetFrame(1);
                }
            }
        }

        public void MarkForStateChange(int moveset, int nextState)
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
            if (!states.ContainsKey(state)) return false;

            if(callOnInterrupt && CurrentState != (int)FighterStateEnum.NULL)
            {
                SetFrame(states[CurrentState].totalFrames+1);
                // ProcessState();
            }

            CurrentStateFrame = stateFrame;
            CurrentState = state;
            if(CurrentStateFrame == 0)
            {
                SetFrame(0);
                //ProcessState();
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
            return fighterManager.definition.movesets[moveset].stateMap[state];
        }

        public void SetMoveset(int movesetIndex)
        {
            throw new NotImplementedException();
        }

        public void SetFrame(int frame)
        {
            CurrentStateFrame = frame;
        }

        public void IncrementFrame(int amt = 1)
        {
            CurrentStateFrame += amt;
        }
    }
}