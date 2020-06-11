using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public interface IStatusEffectable
    {
        void ApplyStatusEffect(StatusEffectDefinition statusEffect);
    }
}