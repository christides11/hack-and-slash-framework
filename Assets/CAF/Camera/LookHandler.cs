using UnityEngine;

namespace CAF.Camera
{
    public interface LookHandler
    {
        /// <summary>
        /// Make the handler look in the given direction.
        /// </summary>
        /// <param name="direction">The direction vector.</param>
        void SetLookDirection(Vector3 direction);

        /// <summary>
        /// Set the look direction to point at the given position.
        /// </summary>
        /// <param name="position">The position to look at.</param>
        void LookAt(Vector3 position);
    }
}