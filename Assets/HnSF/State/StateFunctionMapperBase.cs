using System;
using System.Collections.Generic;
using HnSF.Fighters;

namespace HnSF
{
    [System.Serializable]
    public class StateFunctionMapperBase
    {
        /// <summary>
        /// IFighterBase, IStateVariables, StateTimeline, Current Frame, Frame Range %
        /// </summary>
        public Dictionary<Type, Action<IFighterBase, IStateVariables, StateTimeline, int, float>> functions =
            new Dictionary<Type, Action<IFighterBase, IStateVariables, StateTimeline, int, float>>();

        public virtual bool TryRegisterFunction(Type id, Action<IFighterBase, IStateVariables, StateTimeline, int, float> function)
        {
            if (functions.ContainsKey(id)) return false;
            functions.Add(id, function);
            return true;
        }

        public virtual void RegisterFunction(Type id, Action<IFighterBase, IStateVariables, StateTimeline, int, float> function)
        {
            functions.Add(id, function);
        }

        public virtual bool OverrideFunction(Type id, Action<IFighterBase, IStateVariables, StateTimeline, int, float> function)
        {
            functions[id] = function;
            return true;
        }

        public virtual void RemoveFunction(Type id)
        {
            functions.Remove(id);
        }
    }
}