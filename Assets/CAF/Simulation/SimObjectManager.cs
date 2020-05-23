using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Simulation
{
    public class SimObjectManager
    {
        protected List<SimObject> simObjects = new List<SimObject>();

        /// <summary>
        /// Registers an object to the simulation.
        /// </summary>
        /// <param name="simObject"></param>
        public virtual void RegisterObject(SimObject simObject)
        {
            if (simObjects.Contains(simObject))
            {
                return;
            }
            simObjects.Add(simObject);
            simObject.simObjectManager = this;
        }

        /// <summary>
        /// Instantiate an object and registers it in the simulation.
        /// </summary>
        /// <param name="prefab">The object to instantiate.</param>
        /// <param name="position">The position to instantiate at.</param>
        /// <param name="rotation">The rotation to instantiate with.</param>
        /// <returns></returns>
        public virtual GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject obj = GameObject.Instantiate(prefab, position, rotation);
            RegisterObject(obj.GetComponent<SimObject>());
            return obj;
        }

        /// <summary>
        /// Remove an object from the simulation and destroys it.
        /// </summary>
        /// <param name="simObject">The object to remove.</param>
        public virtual void DestroyObject(SimObject simObject)
        {
            if (simObjects.Contains(simObject))
            {
                simObjects.Remove(simObject);
            }
            GameObject.Destroy(simObject.gameObject);
        }

        /// <summary>
        /// Calls every object's SimUpdate method that is in the simulation.
        /// After that, it ticks the physics engine.
        /// </summary>
        /// <param name="deltatime">The time between this and the last frame.</param>
        public virtual void Update(float deltatime)
        {
            UpdateSimObjects();
            SimulatePhysics(deltatime);
        }

        protected virtual void UpdateSimObjects()
        {
            for (int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimUpdate();
            }
        }

        /// <summary>
        /// Simulate physics.
        /// </summary>
        /// <param name="deltatime">The time between this and the last frame.</param>
        protected virtual void SimulatePhysics(float deltatime)
        {
            Physics.Simulate(deltatime);
        }

        /// <summary>
        /// Calls every object's SimLateUpate method that is in the simulation.
        /// </summary>
        /// <param name="deltatime"></param>
        public virtual void LateUpdate(float deltatime)
        {
            LateUpdateSimObjects();
        }

        protected virtual void LateUpdateSimObjects()
        {
            for (int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimLateUpdate();
            }
        }
    }
}