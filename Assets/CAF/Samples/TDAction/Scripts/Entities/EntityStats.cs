using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public abstract class EntityStats : ScriptableObject
    {
        public float health = 100;

        public int enemyStepLength = 4;
        public float hitstunFrictionGround;
        public float hitstunFrictionAir;

        [Header("Aerial")]
        public float gravity;
        public float hitstunGravity;
        public float maxFallSpeed;
        public float hitstunMaxFallSpeed;
    }
}