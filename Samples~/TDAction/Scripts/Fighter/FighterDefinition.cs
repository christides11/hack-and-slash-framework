using HnSF.Combat;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using UnityEngine;

namespace TDAction.Fighter
{
    [CreateAssetMenu(menuName = "TDA/Entities/Definition")]
    public class FighterDefinition : ScriptableObject
    {
        [System.Serializable]
        public class HurtboxDefinitionItem
        {
            public string name;
            public StateHurtboxDefinition hurtboxDefinition;
        }

        [SerializeField] protected FighterManager entityPrefab;
        public List<Combat.MovesetDefinition> movesets = new List<Combat.MovesetDefinition>();
        public List<HurtboxDefinitionItem> hurtboxDefinitions = new List<HurtboxDefinitionItem>();
    }
}