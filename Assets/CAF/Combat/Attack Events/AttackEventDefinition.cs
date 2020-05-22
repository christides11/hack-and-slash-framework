using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class AttackEventDefinition
    {
        public string nickname = "event";
        public bool active = true;
        public bool onHit;
        public int onHitHitboxGroup;
        public bool onDetect;
        public int onDetectHitboxGroup;
        public uint startFrame = 1;
        public uint endFrame = 1;
        [SerializeReference] public AttackEvent attackEvent;
        public AttackEventVariables variables = new AttackEventVariables();
    }
}