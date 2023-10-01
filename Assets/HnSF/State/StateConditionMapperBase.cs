using System;
using System.Collections.Generic;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionMapperBase
    {
        public Dictionary<Type, Func<IConditionVariables, StateTimeline, StateMachineContext, StateFunctionContext, bool>> functions = new();
        
        public virtual bool TryRegisterCondition(Type id, Func<IConditionVariables, StateTimeline, StateMachineContext, StateFunctionContext, bool> condition)
        {
            if (functions.ContainsKey(id)) return false;
            functions.Add(id, condition);
            return true;
        }

        public virtual void RegisterCondition(Type id, Func<IConditionVariables, StateTimeline, StateMachineContext, StateFunctionContext, bool> condition)
        {
            functions.Add(id, condition);
        }

        public virtual bool OverrideCondition(Type id, Func<IConditionVariables, StateTimeline, StateMachineContext, StateFunctionContext, bool> condition)
        {
            functions[id] = condition;
            return true;
        }

        public virtual void RegisterOrOverrideCondition(Type id, Func<IConditionVariables, StateTimeline, StateMachineContext, StateFunctionContext, bool> condition)
        {
            if (functions.ContainsKey(id))
            {
                functions[id] = condition;
                return;
            }
            functions.Add(id, condition);
        }

        public virtual void RemoveCondition(Type id)
        {
            functions.Remove(id);
        }

        public virtual bool TryCondition(Type id, IConditionVariables variables, StateTimeline timeline, StateMachineContext smContext, StateFunctionContext sfContext)
        {
            return functions[id](variables, timeline, smContext, sfContext);
        }
    }
}