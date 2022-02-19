using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    public class GotoFrameAsset : FighterStateAsset
    {
        public GotoFrameBehaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<GotoFrameBehaviour>.Create(graph, template);
            return playable;
        }
    }
}