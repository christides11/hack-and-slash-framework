using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class HealthManager : MonoBehaviour
    {
        public float Health { get { return health; } }

        [SerializeField] private float health;

        public void SetHealth(float value)
        {
            health = value;
        }

        public void Hurt(float value)
        {
            health -= value;
        }

        public void Heal(float value)
        {
            health += value;
        }
    }
}