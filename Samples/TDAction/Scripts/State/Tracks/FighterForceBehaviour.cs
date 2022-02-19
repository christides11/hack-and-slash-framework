using UnityEngine;

namespace HnSF.Sample.TDAction
{
    [System.Serializable]
    public class ForceSetBehaviour : FighterStateBehaviour
    {
        public ForceSetType forceSetType;
        public Vector2 force = Vector2.zero;
    }
}