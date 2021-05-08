using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class AttackEventVariables
    {
        public List<int> intVars;
        public List<float> floatVars;
        public List<Object> objectVars;
        public List<AnimationCurve> curveVars;
    }
}
