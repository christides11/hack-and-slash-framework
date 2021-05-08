using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class AttackDefinition : ScriptableObject
    {
        [Header("General")]
        #region General
        public string attackName;
        [TextArea(4, 6)]public string description;
        public bool useState = false;
        public ushort stateOverride = 0;
        public int length = 1; //In frames
        #endregion

        [Header("Cancel Windows")]
        #region Cancel Windows
        public List<Vector2Int> commandAttackCancelWindows = new List<Vector2Int>();
        #endregion

        [Header("Groups")]
        [SerializeReference] public List<ChargeDefinition> chargeWindows = new List<ChargeDefinition>();
        [SerializeReference] public List<HitboxGroup> hitboxGroups = new List<HitboxGroup>();
        [SerializeReference] public List<HurtboxGroup> hurtboxGroups = new List<HurtboxGroup>(); 
        [SerializeReference] public List<AttackEventDefinition> events = new List<AttackEventDefinition>();
        [SerializeReference] public List<CancelListDefinition> cancels = new List<CancelListDefinition>();
    }
}