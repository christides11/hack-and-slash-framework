using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    public class IntStateMap
    {
        public string name;
        [SubclassSelector] [SerializeField, SerializeReference]
        public FighterStateReferenceBase state = new FighterStateReferenceBase();
        public StateTimeline stateTimeline;
    }
}