using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TDAction.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;

        public void SetFollowTarget(Transform target)
        {
            followCamera.Follow = target;
            followCamera.LookAt = target;
        }

        public void SetLockonTarget()
        {

        }
    }
}