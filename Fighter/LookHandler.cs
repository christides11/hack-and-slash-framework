using UnityEngine;

namespace HnSF.Fighters
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
        void SetLockOnTarget(FighterBase entityTarget);

        /// <summary>
        /// Set the rotation of the handler.
        /// </summary>
        /// <param name="rotation">The rotation quaternion.</param>
        void SetRotation(Quaternion rotation);

        /// <summary>
        /// Set the rotation of the handler.
        /// </summary>
        /// <param name="rotation">The rotation euler angle.</param>
        void SetRotation(Vector3 rotation);

        /// <summary>
        /// Set the position of the handler.
        /// </summary>
        /// <param name="position">The position.</param>
        void SetPosition(Vector3 position);

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