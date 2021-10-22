﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
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
        public BoxCollectionDefinition hurtboxDefinition;
        #endregion

        [Header("Groups")]
        [SerializeReference] public List<ChargeDefinition> chargeWindows = new List<ChargeDefinition>();
        [SerializeReference] public List<HitboxGroup> hitboxGroups = new List<HitboxGroup>();
        [SerializeReference] public List<AttackEventDefinition> events = new List<AttackEventDefinition>();
        [SerializeReference] public List<CancelListDefinition> cancels = new List<CancelListDefinition>();
    }
}