using CAF.Simulation;
using TDAction.Managers;
using UnityEngine;

namespace TDAction.Simulation
{
    /// <summary>
    /// Registers the object given to the simulation.
    /// </summary>
    public class SimObjectRegister : MonoBehaviour
    {
        [SerializeField] private SimObject simObject;

        private void Start()
        {
            GameManager.instance.GameHandler.simulationObjectManager.RegisterObject(simObject);
        }
    }
}