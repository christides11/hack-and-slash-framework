using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    [System.Serializable]
    public class SimulationManager
    {
        public float TickRate { get; } = 1.0f / 60.0f;
        [field: SerializeField]
        public int Tick { get; protected set; } = 0;
        
        public List<SimulationObject> simObjects = new List<SimulationObject>();
        public GameManager gameManager;
        
        public void Increment()
        {
            for (int i = 0; i < simObjects.Count; i++)
            {
                
            }

            Physics2D.Simulate(TickRate);
            Tick++;
        }

        public void RegisterObject(SimulationObject simObject)
        {
            simObjects.Add(simObject);
            InitializeSimObject(simObject);
            AwakeSimObject(simObject);
        }

        private void InitializeSimObject(SimulationObject simObject)
        {
            foreach (var t in simObject.simulationBehaviours)
            {
                t.SimInitialize(gameManager);
            }

            foreach (var t in simObject.nestedObjects)
            {
                InitializeSimObject(t);
            }
        }
        
        private static void AwakeSimObject(SimulationObject simObject)
        {
            foreach (var t in simObject.simulationBehaviours)
            {
                t.SimAwake();
            }

            foreach (var t in simObject.nestedObjects)
            {
                AwakeSimObject(t);
            }
        }
    }
}