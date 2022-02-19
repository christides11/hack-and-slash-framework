using System.Collections.Generic;
using System.Linq;
using HnSF.Fighters;
using Juce.ImplementationSelector;
using UnityEngine;
using UnityEngine.Playables;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionOR : StateConditionBase
    {
        [SelectImplementation(typeof(StateConditionBase))] [SerializeField, SerializeReference]
        public List<StateConditionBase> conditions = new List<StateConditionBase>();

        public override bool IsTrue(IFighterBase fm)
        {
            for (int i = 0; i < conditions.Count(); i++)
            {
                if (conditions[i].IsTrue(fm)) return true;
            }

            return false;
        }
    }
}