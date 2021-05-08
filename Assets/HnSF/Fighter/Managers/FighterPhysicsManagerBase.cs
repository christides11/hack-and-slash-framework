using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterPhysicsManagerBase : MonoBehaviour
    {
        public bool IsGrounded { get; protected set; } = false;

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

        public virtual void SetGrounded(bool value)
        {
            IsGrounded = value;
        }
    }
}