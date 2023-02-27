using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class SimulationObject : MonoBehaviour
    {
        [HideInInspector] public GameManager gameManager;
        
        public SimulationObject[] nestedObjects;
        public SimulationBehaviour[] simulationBehaviours;
    }
}