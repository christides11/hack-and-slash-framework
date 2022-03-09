using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HnSF
{
    public class FighterStateAsset : PlayableAsset
    {
        [NonSerialized] public TimelineClip clipPassthrough = null;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            throw new NotImplementedException($"{name} does not implement CreatePlayable");
        }
    }
}