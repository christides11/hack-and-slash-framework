using System;
using System.Collections.Generic;
using HnSF.Fighters;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionMapperBase
    {
        public Dictionary<int, Func<IFighterBase, IConditionVariables, bool>> functions =
            new Dictionary<int, Func<IFighterBase, IConditionVariables, bool>>();
        
        public virtual bool TryRegisterCondition(int id, Func<IFighterBase, IConditionVariables, bool> condition)
        {
            if (functions.ContainsKey(id)) return false;
            functions.Add(id, condition);
            return true;
        }

        public virtual void RegisterCondition(int id, Func<IFighterBase, IConditionVariables, bool> condition)
        {
            functions.Add(id, condition);
        }

        public virtual bool OverrideCondition(int id, Func<IFighterBase, IConditionVariables, bool> condition)
        {
            functions[id] = condition;
            return true;
        }

        public virtual void RemoveCondition(int id)
        {
            functions.Remove(id);
        }

        public virtual bool TryCondition(int id, IFighterBase fighter, IConditionVariables variables)
        {
            return functions[id](fighter, variables);
        }
    }
}