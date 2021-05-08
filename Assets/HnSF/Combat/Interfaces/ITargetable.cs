using UnityEngine;

namespace HnSF.Combat
{
    public interface ITargetable
    {
        /// <summary>
        /// If this object is currently targetable.
        /// </summary>
        bool Targetable { get; }
        /// <summary>
        /// The object that's being targeted.
        /// </summary>
        GameObject GetGameObject();
    }
}