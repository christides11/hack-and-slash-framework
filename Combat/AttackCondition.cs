using HnSF.Fighters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    public class AttackCondition
    {
        public bool inverse;

        public virtual bool Result(IFighterBase manager)
        {
            return false;
        }

        public virtual string GetName()
        {
            return "Condition";
        }
    }
}