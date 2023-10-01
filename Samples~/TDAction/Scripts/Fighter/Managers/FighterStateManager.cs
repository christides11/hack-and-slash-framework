using System;
using HnSF.Combat;
using HnSF.Fighters;
using HnSF.Sample.TDAction.State;
using UnityEngine;

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
        private int nextFrame = 0;
        public FighterManager fighterManager;

        [NonSerialized] public StateFunctionMapper functionMapperBase = new StateFunctionMapper(); 
        [NonSerialized] public StateConditionMapper conditionMapperBase = new StateConditionMapper();

        private FighterStateMachineContext smContext = new FighterStateMachineContext();

        private void Awake()
        {
            smContext.fighter = fighterManager;
            functionMapperBase = new StateFunctionMapper();
            conditionMapperBase = new StateConditionMapper();
        }

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
            var topState = state;
            while (true)
            {
                int realFrame = onInterrupt ? state.totalFrames+1 : Mathf.Clamp(CurrentStateFrame, 0, state.totalFrames);
                foreach (var d in state.data)
                {
                    ProcessStateVariables(state, d, realFrame, topState.totalFrames);
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

        private void ProcessStateVariables(StateTimeline timeline, IStateVariables d, int realFrame, int totalFrames)
        {
            var valid = d.FrameRanges.Length == 0 ? true : false;
            int frameRange = 0;
            int frStart = 0;
            int frEnd = 0;
            for (int j = 0; j < d.FrameRanges.Length; j++)
            {
                frStart = ConvertFrameRangeNumber((int)d.FrameRanges[j].x, totalFrames);
                frEnd = ConvertFrameRangeNumber((int)d.FrameRanges[j].y, totalFrames);
                if (realFrame >= frStart && realFrame <= frEnd)
                {
                    frameRange = j;
                    valid = true;
                    break;
                }
            }

            if (!valid) return;
            var varType = d.GetType();
            var sfContext = new StateFunctionContext() { currentFrame = realFrame, frameRangePercent = (float)(realFrame - frStart) / (float)(frEnd - frStart), };
            if (d.Condition != null && !conditionMapperBase.TryCondition(d.Condition.GetType(), d.Condition, timeline, smContext, sfContext)) return;
            functionMapperBase.functions[varType](d, timeline, smContext, sfContext);

            if (d.Children is null) return;
            foreach (var childID in d.Children)
            {
                ProcessStateVariables(timeline, timeline.data[timeline.stateVariablesIDMap[childID]], realFrame, totalFrames);
            }
        }

        private int ConvertFrameRangeNumber(int number, int totalFrames)
        {
            if (number == -1) return totalFrames;
            if (number == -2) return totalFrames + 1;
            return number;
        }

        public void MarkForStateChange(int nextState, int moveset = -1, int frame = 0, bool force = false)
        {
            markedForStateChange = true;
            this.nextMoveset = moveset == -1 ? CurrentStateMoveset : moveset;
            this.nextState = nextState;
            this.nextFrame = frame;
        }

        public IMovesetDefinition GetMoveset(int index)
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
            return (StateTimeline)fighterManager.definition.movesets[CurrentStateMoveset].GetState(CurrentState);
        }
        
        public HnSF.StateTimeline GetState(int state)
        {
            return fighterManager.definition.movesets[CurrentStateMoveset].GetState(state);
        }

        public HnSF.StateTimeline GetState(int moveset, int state)
        {
            return fighterManager.definition.movesets[moveset].GetState(state);
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