using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    [CreateAssetMenu(fileName = "StateHurtbox", menuName = "CAF/StateHurtboxDefinition")]
    public class StateHurtboxDefinition : ScriptableObject
    {
        public List<HurtboxGroup> hurtboxGroups = new List<HurtboxGroup>();
    }
}