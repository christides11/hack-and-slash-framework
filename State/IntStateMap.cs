using UnityEngine;

namespace HnSF
{
    [System.Serializable]
    public class IntStateMap
    {
        public string name;
        [SelectImplementation((typeof(FighterStateReferenceBase)))] [SerializeField, SerializeReference]
        public FighterStateReferenceBase state = new FighterStateReferenceBase();
        public StateTimeline stateTimeline;
    }
}