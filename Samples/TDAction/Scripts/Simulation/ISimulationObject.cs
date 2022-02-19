using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public interface ISimulationObject
    {
        public GameManager Manager { get;  }

        public void SimInitialize(GameManager gameManager);
        public void SimAwake();
        public void SimUpdate();
        public void SimLateUpdate();
    }
}