using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityPhysicsManagerBase : MonoBehaviour
    {

        [SerializeField] protected EntityManager manager;

        public virtual void Tick()
        {

        }

        public virtual void ResetForces()
        {

        }

        /// <summary>
        /// Check if we are on the ground.
        /// </summary>
        public virtual void CheckIfGrounded()
        {

        }
    }
}