using CAF.Entities;
using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class StatusEffectDefinition : ScriptableObject
    {
        public string statusEffectName;
        public int duration;
    }

    [System.Serializable]
    public class StatusEffectDefinition<DataType, StatusEffectType> : StatusEffectDefinition
        where StatusEffectType: StatusEffect<DataType>, new()
    {
        public DataType data;

        public virtual StatusEffectType GetStatusEffect(EntityController target)
        {
            return new StatusEffectType { data = this.data, target = target };
        }
    }
}