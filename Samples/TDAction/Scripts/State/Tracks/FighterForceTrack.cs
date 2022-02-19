using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HnSF.Sample.TDAction
{
    [TrackClipType(typeof(FighterForceMixerBehaviour))]
    public class ForceForceTrack : FighterTrack
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<FighterForceMixerBehaviour>.Create(graph, inputCount);
        }
    }
}