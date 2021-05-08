using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    [System.Serializable]
    public abstract class FighterStat <T>
    {
        [SerializeField] public T baseValue;
        protected T calculatedValue;
        [System.NonSerialized] protected bool isDirty = true;

        public FighterStat(T baseValue)
        {
            this.baseValue = baseValue;
        }

        public T GetCurrentValue()
        {
            if (isDirty)
            {
                calculatedValue = baseValue;
                isDirty = false;
            }
            return calculatedValue;
        }
    }
}