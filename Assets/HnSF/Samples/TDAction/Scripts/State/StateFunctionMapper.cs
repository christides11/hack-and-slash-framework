using System;
using System.Collections.Generic;
using HnSF.Fighters;

namespace HnSF.Sample.TDAction.State
{
    [System.Serializable]
    public class StateFunctionMapper
    {
        public Dictionary<int, Action<IFighterBase, IStateVariables>> functions =
            new Dictionary<int, Action<IFighterBase, IStateVariables>>();
        
        public StateFunctionMapper()
        {
            functions.Add((int)StateFunctionEnum.CHANGE_STATE, BaseStateFunctions.ChangeState);
        }
        
        public virtual bool TryRegisterFunction(int id, Action<IFighterBase, IStateVariables> function)
        {
            if (functions.ContainsKey(id)) return false;
            functions.Add(id, function);
            return true;
        }

        public virtual void RegisterFunction(int id, Action<IFighterBase, IStateVariables> function)
        {
            functions.Add(id, function);
        }

        public virtual bool OverrideFunction(int id, Action<IFighterBase, IStateVariables> function)
        {
            functions[id] = function;
            return true;
        }

        public virtual void RemoveFunction(int id)
        {
            functions.Remove(id);
        }
    }
}