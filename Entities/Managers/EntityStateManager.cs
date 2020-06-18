using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityStateManager : MonoBehaviour
    {
        public EntityState CurrentState { get { return currentState; } }
        public uint CurrentStateFrame { get { return currentStateFrame; } }

        [SerializeField] protected EntityManager controller = null;
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
            state.Controller = controller;
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
                if (callOnInterrupt)
                {
                    if (currentState != null)
                    {
                        currentState.OnInterrupted();
                    }
                }
                currentStateFrame = stateFrame;
                currentState = states[state];
                if (currentStateFrame == 0)
                {
                    currentState.Initialize();
                }
                currentStateName = currentState.GetName();
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
            currentStateFrame = stateFrame;
            if (callOnInterrupt)
            {
                currentState.OnInterrupted();
            }

            currentState = state;
            state.Controller = controller;
            if (currentStateFrame == 0)
            {
                currentState.Initialize();
            }
        }

        public virtual void SetFrame(uint frame)
        {
            currentStateFrame = frame;
        }

        public virtual void IncrementFrame()
        {
            currentStateFrame++;
        }
    }
}