using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class CancelListDefinition
    {
        public int startFrame = 1;
        public int endFrame = 1;
        public int cancelListID;
        [SerializeReference] public List<AttackCondition> conditions = new List<AttackCondition>();
    }
}