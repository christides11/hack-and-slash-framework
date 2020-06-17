using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Managers
{
    public class GameHandler
    {
        public SimObjectManager simulationObjectManager;
        public TimeStepManager timeStepManager;

        public GameHandler()
        {
            simulationObjectManager = new SimObjectManager();
            timeStepManager = new TimeStepManager(60.0f, 1.0f, 120.0f, 30.0f);
        }
    }
}