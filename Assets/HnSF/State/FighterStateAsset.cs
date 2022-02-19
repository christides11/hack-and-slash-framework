using System;
using UnityEngine;
using UnityEngine.Playables;

namespace HnSF
{
    public class FighterStateAsset : PlayableAsset
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            throw new NotImplementedException($"{name} does not implement CreatePlayable.");
        }
    }
}