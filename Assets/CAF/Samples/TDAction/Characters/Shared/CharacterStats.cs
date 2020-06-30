using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CharacterStats : EntityStats
    {
        public int jumpSquatFrames = 4;

        [Header("Ground")]
        public float groundFriction;

        [Header("Walk")]
        public float walkBaseAcceleration;
        public float walkAcceleration;
        public float walkMaxSpeed;

        [Header("Run")]
        public float runBaseAcceleration;
        public float runAcceleration;
        public float runMaxSpeed;

        [Header("Aerial")]
        public float airBaseAcceleration;
        public float airAcceleration;
        public float airMaxSpeed;
        public float aerialFriction;
        public float shortHopForce;
        public float fullHopForce;
    }
}