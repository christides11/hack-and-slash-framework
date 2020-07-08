using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using TDAction.Managers;
using UnityEngine;

namespace TDAction.Simulation
{
    public class SimObjectRegister : MonoBehaviour
    {
        [SerializeField] private List<SimObject> objects = new List<SimObject>();

        private void Start()
        {
            foreach (SimObject so in objects) {
                GameManager.instance.GameHandler.simulationObjectManager.RegisterObject(so);
            }
        }
    }
}