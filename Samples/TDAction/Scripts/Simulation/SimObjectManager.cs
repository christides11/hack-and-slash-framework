using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Simulation
{
    public class SimObjectManager : CAF.Simulation.SimObjectManager
    {
        protected override void SimulatePhysics(float deltatime)
        {
            Physics2D.Simulate(deltatime);
        }
    }
}