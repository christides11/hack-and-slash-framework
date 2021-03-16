using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Simulation
{
    public interface ISimObjectManager
    {
        void RegisterObject(SimObject simObject);
        GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation);
        void DestroyObject(SimObject simObject);
    }
}