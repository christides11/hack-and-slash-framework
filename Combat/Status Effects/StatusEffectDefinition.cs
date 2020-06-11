using CAF.Entities;
using UnityEngine;

namespace CAF.Combat
{
    public abstract class StatusEffectDefinition : ScriptableObject
    {
        public string statusEffectName;
        public int duration;
    }

    public class StatusEffectDefinition<DataType, StatusEffectType> : StatusEffectDefinition
        where StatusEffectType: StatusEffect<DataType>, new()
    {
        public DataType data;

        public StatusEffectType GetStatusEffect(EntityController target)
        {
            return new StatusEffectType { data = this.data, target = target };
        }
    }
}