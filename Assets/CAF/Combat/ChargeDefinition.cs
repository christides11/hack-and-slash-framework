using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class ChargeDefinition
    {
        public int frame = 1;
        public bool releaseOnCompletion = true;
        public List<ChargeLevel> chargeLevels = new List<ChargeLevel>();
    }
}