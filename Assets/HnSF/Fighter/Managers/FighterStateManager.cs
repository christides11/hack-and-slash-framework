using System.Collections.Generic;
using UnityEngine;
using static HnSF.Fighters.IFighterStateManager;

namespace HnSF.Fighters
{
    public class FighterStateManager : MonoBehaviour, IFighterStateManager
    {
        public delegate void StateAction(IFighterBase self, ushort from, uint fromStateFrame);
        public delegate void StateFrameAction(IFighterBase self, uint preChangeFrame);
        public event StateAction OnStatePreChange;
        public event StateAction OnStatePostChange;
        public event StateFrameAction OnStateFrameSet;

        public virtual IFighterBase Manager { get; }
        public ushort CurrentState { get { return currentState; } }
        public uint CurrentStateFrame { get { return currentStateFrame; } }

        protected Dictionary<ushort, FighterStateBase> states = new Dictionary<ushort, FighterStateBase>();
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

        public virtual void LateTick()
        {
            states[currentState].OnLateUpdate();
        }

        /// <summary>
        /// Adds a state to the entity's state list.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="stateNumber">The number of the state.</param>
        public virtual void AddState(FighterStateBase state, ushort stateNumber)
        {
            states.Add(stateNumber, state);
        }

        public void RemoveState(ushort stateNumber)
        {
            if (CurrentState == stateNumber)
            {
                return;
            }
            states.Remove(stateNumber);
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
                OnStatePreChange?.Invoke(Manager, oldState, oldStateFrame);
                if (currentStateFrame == 0)
                {
                    states[currentState].Initialize();
                    currentStateFrame = 1;
                }
                currentStateName = states[currentState].GetName();
                OnStatePostChange?.Invoke(Manager, oldState, oldStateFrame);
                return true;
            }
            return false;
        }

        public virtual FighterStateBase GetState(ushort state)
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
            OnStateFrameSet?.Invoke(Manager, preFrame);
        }

        public virtual void IncrementFrame()
        {
            currentStateFrame++;
        }
    }
}