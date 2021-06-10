using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    public interface IHurtable
    {
        int GetTeam();
        HitReactionBase Hurt(HurtInfoBase hurtInfo);
        void Heal(HealInfoBase healInfo);
    }
}