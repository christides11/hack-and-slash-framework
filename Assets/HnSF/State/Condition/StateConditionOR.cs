using System.Collections.Generic;
using System.Linq;
using HnSF.Fighters;
using UnityEngine;

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