using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterStateManager : MonoBehaviour
    {
        public delegate void StateAction(FighterBase self, ushort from, uint fromStateFrame);
        public delegate void StateFrameAction(FighterBase self, uint preChangeFrame);
        public event StateAction OnStatePreChange;
        public event StateAction OnStatePostChange;
        public event StateFrameAction OnStateFrameSet;

        public ushort CurrentState { get { return currentState; } }
        public uint CurrentStateFrame { get { return currentStateFrame; } }

        [SerializeField] protected FighterBase manager = null;
        protected Dictionary<ushort, FighterState> states = new Dictionary<ushort, FighterState>();
        protected ushort currentState;
        [SerializeField] protected uint currentStateFrame = 0;
        [SerializeField] protected string currentStateName;

        public virtual void Tick()
        {
            if (!states.ContainsKey(currentState))
            {
                return;
            }
            states[currentState].OnUpdate();
        }

        /// <summary>
        /// Adds a state to the entity's state list.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="stateNumber">The number of the state.</param>
        public virtual void AddState(FighterState state, ushort stateNumber)
        {
            state.Manager = manager;
            states.Add(stateNumber, state);
        }

        /// <summary>
        /// Changes the state to the given one.
        /// </summary>
        /// <param name="state">The state to change to.</param>
        /// <param name="stateFrame">What frame to start the state at.</param>
        /// <param name="callOnInterrupt">If OnInterrupt of the current state should be called.</param>
        /// <returns></returns>
        public virtual bool ChangeState(ushort state, uint stateFrame = 0, bool callOnInterrupt = true)
        {
            if (states.ContainsKey(state))
            {
                ushort oldState = currentState;
                uint oldStateFrame = currentStateFrame;

                if (callOnInterrupt)
                {
                    if (states.ContainsKey(currentState))
                    {
                        states[currentState].OnInterrupted();
                    }
                }
                currentStateFrame = stateFrame;
                currentState = state;
                OnStatePreChange?.Invoke(manager, oldState, oldStateFrame);
                if (currentStateFrame == 0)
                {
                    states[currentState].Initialize();
                    currentStateFrame = 1;
                }
                currentStateName = states[currentState].GetName();
                OnStatePostChange?.Invoke(manager, oldState, oldStateFrame);
                return true;
            }
            return false;
        }

        public virtual FighterState GetState(ushort state)
        {
            if (states.ContainsKey(state))
            {
                return states[state];
            }
            return null;
        }

        public virtual void SetFrame(uint frame)
        {
            uint preFrame = currentStateFrame;
            currentStateFrame = frame;
            OnStateFrameSet?.Invoke(manager, preFrame);
        }

        public virtual void IncrementFrame()
        {
            currentStateFrame++;
        }
    }
}