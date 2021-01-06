using CAF.Entities;
using UnityEngine;

namespace CAF.Camera
{
    /// <summary>
    /// The LookHandler is used to defined a camera-like object.
    /// This is useful for objects that don't need an actual camera,
    /// but do need to move based on some sort of "camera."
    /// </summary>
    public interface LookHandler
    {
        /// <summary>
        /// Set the target that we should be tracking.
        /// </summary>
        /// <param name="target"></param>
        void SetLookAtTarget(Transform target);

        /// <summary>
        /// Set the target that we should lock on to.
        /// </summary>
        /// <param name="entityTarget"></param>
        void SetLockOnTarget(EntityManager entityTarget);

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

        /// <summary>
        /// The transform that is used to define what direction we're currently looking in.
        /// </summary>
        /// <returns>The transform.</returns>
        Transform LookTransform();

        /// <summary>
        /// Resets the lookhandler to a wanted set position.
        /// </summary>
        void Reset();
    }
}