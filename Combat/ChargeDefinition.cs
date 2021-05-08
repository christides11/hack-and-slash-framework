using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class ChargeDefinition
    {
        public int startFrame = 1;
        public int endFrame = 1;
        public bool releaseOnCompletion = true;
        public List<ChargeLevel> chargeLevels = new List<ChargeLevel>();
    }
}