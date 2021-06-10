using HnSF.Fighters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    public class AttackCondition
    {
        public virtual bool Result(FighterBase manager)
        {
            return false;
        }

        public virtual string GetName()
        {
            return "Condition";
        }
    }
}