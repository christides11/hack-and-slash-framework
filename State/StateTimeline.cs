using UnityEngine;
using UnityEngine.Timeline;

namespace HnSF
{
    [CreateAssetMenu(fileName = "StateTimeline", menuName = "HnSF/StateTimeline")]
    public class StateTimeline : TimelineAsset
    {
        public int totalFrames = 0;
    }
}