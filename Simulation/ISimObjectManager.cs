using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Simulation
{
    public interface ISimObjectManager
    {
        void RegisterObject(ISimObject simObject);
        GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation);
        void RemoveObjectFromSimulation(ISimObject simObject);
    }
}