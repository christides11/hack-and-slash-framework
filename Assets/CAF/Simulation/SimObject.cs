using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Simulation
{
    /// <summary>
    /// A SimObject should be implemented by any class that will be apart of the simulation.
    /// </summary>
    public class SimObject : MonoBehaviour
    {
        /// <summary>
        /// The SimObjectManager this is attached to. Assigned once it's added to the simulation.
        /// </summary>
        public SimObjectManager simObjectManager;

        protected virtual void Awake()
        {
            SimAwake();
        }

        protected virtual void Start()
        {
            SimStart();
        }

        /// <summary>
        /// Called once as soon as the script is initialized.
        /// </summary>
        public virtual void SimAwake()
        {
        }

        /// <summary>
        /// Called once after awake.
        /// </summary>
        public virtual void SimStart()
        {
        }

        /// <summary>
        /// Called every simulations tick.
        /// </summary>
        /// <param name="deltaTime">the time between the last frame and this one.</param>
        public virtual void SimUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// Called every simulation tick after all updates are called.
        /// </summary
        /// <param name="deltaTime">the time between the last frame and this one.</param>
        public virtual void SimLateUpdate(float deltaTime)
        {
        }
    }
}