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

        [NonSerialized] public StateFunctionMapper functionMapperBase = new StateFunctionMapper(); 
        [NonSerialized] public StateConditionMapper conditionMapperBase = new StateConditionMapper();

        public void Tick()
        {
            if (markedForStateChange)
            {
                ChangeState(nextState, 0, true);
            }
            if (CurrentState == 0) return;
            ProcessState(states[CurrentState], states[CurrentState].autoIncrement, states[CurrentState].autoLoop);
        }

        private void ProcessState(StateTimeline state, bool onInterrupt = false, bool autoIncrement = false, bool autoLoop = false)
        {
            
            while (true)
            {
                int realFrame = onInterrupt ? state.totalFrames+1 : Mathf.Clamp(CurrentStateFrame, 0, state.totalFrames);
                foreach (var d in state.data)
                {
                    var valid = true;
                    for (int j = 0; j < d.FrameRanges.Length; j++)
                    {
                        if (!(realFrame < d.FrameRanges[j].x) &&
                            !(realFrame > d.FrameRanges[j].y)) continue;
                        valid = false;
                        break;
                    }

                    if (!valid) continue;
                    if (!conditionMapperBase.TryCondition(d.Condition.FunctionMap, fighterManager, d.Condition)) continue;
                    functionMapperBase.functions[d.FunctionMap](fighterManager, d);
                }

                if (!state.useBaseState) break;
                state = (StateTimeline)state.baseState;
            }

            if (onInterrupt != false || !autoIncrement) return;
            IncrementFrame(1);
            if (autoLoop && CurrentStateFrame > state.totalFrames)
            {
                SetFrame(1);
            }
        }

        public void MarkForStateChange(int moveset, int nextState)
        {
            markedForStateChange = true;
            this.nextState = nextState;
        }

        public Combat.MovesetDefinition GetMoveset(int index)
        {
            return fighterManager.definition.movesets[index];
        }

        public bool ChangeState(int state, int stateFrame = 0, bool callOnInterrupt = true)
        {
            markedForStateChange = false;
            if (!states.ContainsKey(state)) return false;

            if(callOnInterrupt && CurrentState != (int)FighterStateEnum.NULL)
            {
                SetFrame(states[CurrentState].totalFrames+1);
                ProcessState(states[CurrentState], true);
            }

            CurrentStateFrame = stateFrame;
            CurrentState = state;
            if(CurrentStateFrame == 0)
            {
                SetFrame(0);
                ProcessState(states[CurrentState]);
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
            CurrentStateMoveset = movesetIndex;
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