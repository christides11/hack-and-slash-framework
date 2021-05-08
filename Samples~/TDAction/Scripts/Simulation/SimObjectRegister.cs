using HnSF.Simulation;
using TDAction.Managers;
using UnityEngine;

namespace TDAction.Simulation
{
    /// <summary>
    /// Registers the object given to the simulation.
    /// </summary>
    public class SimObjectRegister : MonoBehaviour
    {
        private void Start()
        {
            GameManager.instance.GameHandler.simulationObjectManager.RegisterObject(GetComponent<ISimObject>());
        }
    }
}