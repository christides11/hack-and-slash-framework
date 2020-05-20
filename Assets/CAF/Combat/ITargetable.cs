using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    public interface ITargetable
    {
        /// <summary>
        /// If this object is currently targetable.
        /// </summary>
        bool Targetable { get; }
        /// <summary>
        /// The center of the object in world sace.
        /// </summary>
        Vector3 Center { get; }
    }
}