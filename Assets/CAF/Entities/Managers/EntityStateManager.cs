using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityStateManager : MonoBehaviour
    {
        public delegate void StateAction(EntityManager self, EntityState from, uint fromStateFrame);
        public delegate void StateFrameAction(EntityManager self, uint preChangeFrame);
        public event StateAction OnStatePreChange;
        public event StateAction OnStatePostChange;
        public event StateFrameAction OnStateFrameSet;

        public EntityState CurrentState { get { return currentState; } }
        public uint CurrentStateFrame { get { return currentStateFrame; } }

        [SerializeField] protected EntityManager manager = null;
        protected Dictionary<int, EntityState> states = new Dictionary<int, EntityState>();
        protected EntityState currentState;
        [SerializeField] protected uint currentStateFrame = 0;
        [SerializeField] protected string currentStateName;

        public virtual void Tick()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        /// <summary>
        /// Adds a state to the entity's state list.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="stateNumber">The number of the state.</param>
        public virtual void AddState(EntityState state, int stateNumber)
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
        public virtual bool ChangeState(int state, uint stateFrame = 0, bool callOnInterrupt = true)
        {
            if (states.ContainsKey(state))
            {
                EntityState oldState = currentState;
                uint oldStateFrame = currentStateFrame;

                if (callOnInterrupt)
                {
                    if (currentState != null)
                    {
                        currentState.OnInterrupted();
                    }
                }
                currentStateFrame = stateFrame;
                currentState = states[state];
                OnStatePreChange?.Invoke(manager, oldState, oldStateFrame);
                if (currentStateFrame == 0)
                {
                    currentState.Initialize();
                    currentStateFrame = 1;
                }
                currentStateName = currentState.GetName();
                OnStatePostChange?.Invoke(manager, oldState, oldStateFrame);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Changes the state to the given one.
        /// </summary>
        /// <param name="state">The state to change to.</param>
        /// <param name="stateFrame">What frame to start the state at.</param>
        /// <param name="callOnInterrupt">If OnInterrupt of the current state should be called.</param>
        public virtual void ChangeState(EntityState state, uint stateFrame = 0, bool callOnInterrupt = true)
        {
            EntityState oldState = currentState;
            uint oldStateFrame = currentStateFrame;

            if (callOnInterrupt)
            {
                currentState.OnInterrupted();
            }
            currentStateFrame = stateFrame;
            currentState = state;
            state.Manager = manager;
            OnStatePreChange?.Invoke(manager, oldState, oldStateFrame);
            if (currentStateFrame == 0)
            {
                currentState.Initialize();
                currentStateFrame = 1;
            }
            currentStateName = currentState.GetName();
            OnStatePostChange?.Invoke(manager, oldState, oldStateFrame);
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