using System;
using System.Collections.Generic;

namespace HnSF
{
    [System.Serializable]
    public class StateFunctionMapperBase
    {
        /// <summary>
        /// IStateVariables, StateTimeline
        /// </summary>
        public Dictionary<Type, Action<IStateVariables, StateTimeline, StateMachineContext, StateFunctionContext>> functions = new();

        public virtual bool TryRegisterFunction(Type id, Action<IStateVariables, StateTimeline, StateMachineContext, StateFunctionContext> function)
        {
            if (functions.ContainsKey(id)) return false;
            functions.Add(id, function);
            return true;
        }

        public virtual void RegisterFunction(Type id, Action<IStateVariables, StateTimeline, StateMachineContext, StateFunctionContext> function)
        {
            functions.Add(id, function);
        }

        public virtual bool OverrideFunction(Type id, Action<IStateVariables, StateTimeline, StateMachineContext, StateFunctionContext> function)
        {
            functions[id] = function;
            return true;
        }

        public virtual void RegisterOrOverrideFunction(Type id, Action<IStateVariables, StateTimeline, StateMachineContext, StateFunctionContext> function)
        {
            if (functions.ContainsKey(id))
            {
                functions[id] = function;
                return;
            }
            functions.Add(id, function);
        }

        public virtual void RemoveFunction(Type id)
        {
            functions.Remove(id);
        }

        public virtual void TryFunction(Type id, IStateVariables variables, StateTimeline timeline, StateMachineContext smContext, StateFunctionContext sfContext)
        {
            functions[id](variables, timeline, smContext, sfContext);
        }
    }
}