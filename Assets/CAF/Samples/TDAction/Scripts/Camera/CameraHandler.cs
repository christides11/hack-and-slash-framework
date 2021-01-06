﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CAF.Camera;
using CAF.Entities;

namespace TDAction.Camera
{
    public class CameraHandler : MonoBehaviour, LookHandler
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;

        public void LookAt(Vector3 position)
        {

        }

        public Transform LookTransform()
        {
            return transform;
        }

        public void Reset()
        {

        }

        public void SetLookAtTarget(Transform target)
        {
            followCamera.Follow = target;
            followCamera.LookAt = target;
        }

        public void SetLockOnTarget(EntityManager entityTarget)
        {

        }

        public void SetLookDirection(Vector3 direction)
        {

        }
    }
}