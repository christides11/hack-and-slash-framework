using HnSF.Input;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class AttackEventDefinition
    {
        public string nickname = "event";
        public bool active = true;
        public OnHitType onHitCheck;
        public int onHitHitboxGroup;
        public int onHitIDGroup;
        public int startFrame = 1;
        public int endFrame = 1;
        [SerializeReference] public AttackEvent attackEvent;
        public AttackEventVariables variables = new AttackEventVariables();
        public AttackEventInputCheckTiming inputCheckTiming = AttackEventInputCheckTiming.NONE;
        public int inputCheckStartFrame = 1;
        public int inputCheckEndFrame = 1;
        public InputSequence input = new InputSequence();

        public int chargeLevelMin = 0;
        public int chargeLevelMax = 0;

        public bool inputCheckProcessed;
    }
}