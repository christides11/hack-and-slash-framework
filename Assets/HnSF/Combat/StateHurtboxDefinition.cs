using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [CreateAssetMenu(fileName = "StateHurtbox", menuName = "HnSF/StateHurtboxDefinition")]
    public class StateHurtboxDefinition : ScriptableObject
    {
        [SerializeReference] public List<HurtboxGroup> hurtboxGroups = new List<HurtboxGroup>();
    }
}