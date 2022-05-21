using System;
using System.Collections.Generic;
using HnSF.Fighters;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionMapperBase
    {
        public Dictionary<Type, Func<IFighterBase, IConditionVariables, StateTimeline, int, bool>> functions =
            new Dictionary<Type, Func<IFighterBase, IConditionVariables, StateTimeline, int, bool>>();
        
        public virtual bool TryRegisterCondition(Type id, Func<IFighterBase, IConditionVariables, StateTimeline, int, bool> condition)
        {
            if (functions.ContainsKey(id)) return false;
            functions.Add(id, condition);
            return true;
        }

        public virtual void RegisterCondition(Type id, Func<IFighterBase, IConditionVariables, StateTimeline, int, bool> condition)
        {
            functions.Add(id, condition);
        }

        public virtual bool OverrideCondition(Type id, Func<IFighterBase, IConditionVariables, StateTimeline, int, bool> condition)
        {
            functions[id] = condition;
            return true;
        }

        public virtual void RemoveCondition(Type id)
        {
            functions.Remove(id);
        }

        public virtual bool TryCondition(Type id, IFighterBase fighter, IConditionVariables variables, StateTimeline timeline, int frame)
        {
            return functions[id](fighter, variables, timeline, frame);
        }
    }
}