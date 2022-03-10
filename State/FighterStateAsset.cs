using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HnSF
{
    [System.Serializable]
    public class FighterStateAsset : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }
        
        [NonSerialized] public TimelineClip clipPassthrough = null;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            throw new NotImplementedException($"{name} does not implement CreatePlayable");
        }
    }
}