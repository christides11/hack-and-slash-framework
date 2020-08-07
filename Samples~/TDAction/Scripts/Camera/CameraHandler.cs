using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TDAction.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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