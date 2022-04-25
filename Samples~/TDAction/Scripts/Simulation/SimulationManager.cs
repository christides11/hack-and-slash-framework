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
        
        public List<ISimulationObject> simObjects = new List<ISimulationObject>();
        public GameManager gameManager;
        
        public void Increment()
        {
            for (int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimUpdate();
            }

            Physics2D.Simulate(TickRate);
            Tick++;
        }

        public void RegisterObject(ISimulationObject simObject)
        {
            simObject.SimInitialize(gameManager);
            simObject.SimAwake();
            simObjects.Add(simObject);
        }
    }
}