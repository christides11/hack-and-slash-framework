using CAF.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public abstract class StatusEffect
    {
        public EntityController target;

        public abstract void Apply();
    }

    public abstract class StatusEffect<DataType> : StatusEffect
    {
        public DataType data;
    }
}