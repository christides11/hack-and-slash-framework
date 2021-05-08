using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterPhysicsManagerBase : MonoBehaviour
    {

        [SerializeField] protected FighterBase manager;

        public virtual void Tick()
        {

        }

        public virtual void Freeze()
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