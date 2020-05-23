using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public interface IHurtable
    {
        int Team { get; }

        HitReaction Hurt(Vector3 center, Vector3 forward, Vector3 right);
        void Heal();
    }
}