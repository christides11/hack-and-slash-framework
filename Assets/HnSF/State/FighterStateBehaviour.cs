using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HnSF
{
    [System.Serializable]
    public class FighterStateBehaviour : PlayableBehaviour
    {
        [NonSerialized] public TimelineClip timelineClip;

        [SelectImplementation(typeof(StateConditionBase))] [SerializeField, SerializeReference]
        public StateConditionBase conditon = new StateConditionBoolean();
    }
}