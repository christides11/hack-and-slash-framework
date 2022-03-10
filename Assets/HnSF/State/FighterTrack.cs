using HnSF.Fighters;
using UnityEngine.Timeline;

namespace HnSF
{
    [System.Serializable]
    [TrackBindingType(typeof(IFighterBase))]
    [TrackClipType(typeof(FighterStateAsset))]
    public class FighterTrack : TrackAsset{ }
}