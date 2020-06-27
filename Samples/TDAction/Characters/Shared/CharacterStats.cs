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
    }
}