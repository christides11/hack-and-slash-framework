using UnityEngine;

namespace CAF.Combat
{
    public interface ITargetable
    {
        /// <summary>
        /// If this object is currently targetable.
        /// </summary>
        bool Targetable { get; }
        /// <summary>
        /// The center of the object in world space.
        /// </summary>
        Vector3 GetCenter();
    }
}