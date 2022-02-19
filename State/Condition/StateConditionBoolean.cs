using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using Juce.ImplementationSelector;
using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    [SelectImplementationCustomDisplayName("Menu/boolean")]
    public class StateConditionBoolean : StateConditionBase
    {
        public bool trueFalse = true;

        public override bool IsTrue(IFighterBase fm)
        {
            return trueFalse;
        }
    }
}