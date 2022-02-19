using System.Collections.Generic;
using System.Linq;
using HnSF.Fighters;
using HnSF.Sample.TDAction;
using Juce.ImplementationSelector;
using UnityEngine;
using UnityEngine.Playables;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionAND : StateConditionBase
    {
        [SelectImplementation(typeof(StateConditionBase))] [SerializeField, SerializeReference]
        public List<StateConditionBase> conditions = new List<StateConditionBase>();

        public override bool IsTrue(IFighterBase fm)
        {
            for (int i = 0; i < conditions.Count(); i++)
            {
                if (conditions[i].IsTrue(fm) == false) return false;
            }

            return true;
        }
    }
}