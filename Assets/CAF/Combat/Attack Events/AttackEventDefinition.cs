using CAF.Input;
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
        public uint startFrame = 1;
        public uint endFrame = 1;
        [SerializeReference] public AttackEvent attackEvent;
        public AttackEventVariables variables = new AttackEventVariables();
        public AttackEventInputCheckTiming inputCheckTiming = AttackEventInputCheckTiming.NONE;
        public uint inputCheckStartFrame = 1;
        public uint inputCheckEndFrame = 1;
        public InputSequence input = new InputSequence();

        public bool inputCheckProcessed;
    }
}