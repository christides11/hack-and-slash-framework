using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    public class IntStateMap
    {
        [SelectImplementation((typeof(FighterStateReferenceBase)))] [SerializeField, SerializeReference]
        public FighterStateReferenceBase state = new FighterStateReferenceBase();
        public StateTimeline stateTimeline;
    }
}