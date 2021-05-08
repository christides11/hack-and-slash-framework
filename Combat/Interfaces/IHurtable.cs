using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    public interface IHurtable
    {
        int GetTeam();
        HitReaction Hurt(HurtInfoBase hurtInfo);
        void Heal(HealInfoBase healInfo);
    }
}