using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    public class FighterForceAsset : FighterStateAsset
    {
        public ForceSetBehaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ForceSetBehaviour>.Create(graph, template);
            return playable;
        }
    }
}