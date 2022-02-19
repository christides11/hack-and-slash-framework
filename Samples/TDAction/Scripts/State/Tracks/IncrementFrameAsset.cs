using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    public class IncrementFrameAsset : FighterStateAsset
    {
        public IncrementFrameBehaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<IncrementFrameBehaviour>.Create(graph, template);
            return playable;
        }
    }
}