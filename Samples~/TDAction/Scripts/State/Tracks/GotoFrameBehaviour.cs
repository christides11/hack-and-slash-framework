using UnityEngine;
using UnityEngine.Playables;

namespace HnSF.Sample.TDAction
{
    [System.Serializable]
    public class GotoFrameBehaviour : FighterStateBehaviour
    {
        public int frame = 1;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            FighterManager cm = playerData as FighterManager;
            if (cm == null) return;
            cm.stateManager.SetFrame(frame);
        }
    }
}