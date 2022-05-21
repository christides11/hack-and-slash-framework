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

        private bool markedForStateChange = false;
        private int nextMoveset = -1;
        private int nextState = 0;
        public FighterManager fighterManager;

        [NonSerialized] public StateFunctionMapper functionMapperBase = new StateFunctionMapper(); 
        [NonSerialized] public StateConditionMapper conditionMapperBase = new StateConditionMapper();

        public void Tick()
        {
            if (markedForStateChange)
            {
                ChangeState(state: nextState, moveset: nextMoveset, stateFrame: 0, callOnInterrupt: true);
            }
            if (CurrentState == 0) return;
            var stateTimeline = GetState();
            ProcessState(stateTimeline, onInterrupt: false, autoIncrement: stateTimeline.autoIncrement, autoLoop: stateTimeline.autoLoop);
        }

        private void ProcessState(StateTimeline state, bool onInterrupt = false, bool autoIncrement = false, bool autoLoop = false)
        {
            while (true)
            {
                int realFrame = onInterrupt ? state.totalFrames+1 : Mathf.Clamp(CurrentStateFrame, 0, state.totalFrames);
                foreach (var d in state.data)
                {
                    ProcessStateVariables(state, d, realFrame);
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

        private void ProcessStateVariables(StateTimeline timeline, IStateVariables d, int realFrame)
        {
            var valid = true;
            for (int j = 0; j < d.FrameRanges.Length; j++)
            {
                if (!(realFrame < d.FrameRanges[j].x) &&
                    !(realFrame > d.FrameRanges[j].y)) continue;
                valid = false;
                break;
            }

            if (!valid) return;
            var varType = d.GetType();
            if (!conditionMapperBase.TryCondition(varType, fighterManager, d.Condition, timeline, realFrame)) return;
            functionMapperBase.functions[varType](fighterManager, d, timeline, realFrame);

            foreach (var t in d.Children)
            {
                ProcessStateVariables(timeline, t, realFrame);
            }
        }

        public void MarkForStateChange(int nextState, int moveset = -1)
        {
            markedForStateChange = true;
            this.nextMoveset = moveset == -1 ? CurrentStateMoveset : moveset;
            this.nextState = nextState;
        }

        public Combat.MovesetDefinition GetMoveset(int index)
        {
            return fighterManager.definition.movesets[index];
        }

        public bool ChangeState(int state, int moveset = -1, int stateFrame = 0, bool callOnInterrupt = true)
        {
            markedForStateChange = false;
            
            if(callOnInterrupt && CurrentState != (int)FighterStateEnum.NULL)
            {
                StateTimeline currentStateTimeline = GetState();
                SetFrame(currentStateTimeline.totalFrames+1);
                ProcessState(currentStateTimeline, true);
            }

            CurrentStateFrame = stateFrame;
            CurrentStateMoveset = moveset == -1 ? CurrentStateMoveset : moveset;
            CurrentState = state;
            if(CurrentStateFrame == 0)
            {
                SetFrame(0);
                ProcessState(GetState());
                SetFrame(1);
            }

            return true;
        }
        
        public void InitState()
        {
            if (CurrentState == 0) return;
            SetFrame(0);
        }

        public StateTimeline GetState()
        {
            return (StateTimeline)fighterManager.definition.movesets[CurrentStateMoveset].stateMap[CurrentState];
        }
        
        public HnSF.StateTimeline GetState(int state)
        {
            return fighterManager.definition.movesets[CurrentStateMoveset].stateMap[state];
        }

        public HnSF.StateTimeline GetState(int moveset, int state)
        {
            return fighterManager.definition.movesets[moveset].stateMap[state];
        }

        public void SetMoveset(int index)
        {
            CurrentStateMoveset = index;
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