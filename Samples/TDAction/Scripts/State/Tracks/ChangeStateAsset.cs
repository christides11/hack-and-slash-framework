using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    [System.Serializable]
    public class ChangeStateAsset : FighterStateAsset
    {
        public ChangeStateBehaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ChangeStateBehaviour>.Create(graph, template);
            return playable;
        }
    }
}