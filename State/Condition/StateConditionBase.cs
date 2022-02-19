using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionBase
    {
        public bool inverse = false;

        public virtual bool IsTrue(IFighterBase fm)
        {
            return !inverse;
        }
    }
}