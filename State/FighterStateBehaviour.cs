using Juce.ImplementationSelector;
using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    [System.Serializable]
    public class FighterStateBehaviour : PlayableBehaviour
    {
        [SelectImplementation(typeof(StateConditionBase))] [SerializeField, SerializeReference]
        public StateConditionBase conditon = new StateConditionBoolean();
    }
}