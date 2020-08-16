using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using TDAction.Managers;
using UnityEngine;

namespace TDAction.Simulation
{
    public class SimObjectRegister : MonoBehaviour
    {
        [SerializeField] private SimObject simObject;

        private void Start()
        {
            GameManager.instance.GameHandler.simulationObjectManager.RegisterObject(simObject);
        }
    }
}