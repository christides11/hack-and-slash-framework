using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStatsManager : MonoBehaviour
    {
        [SerializeField] protected FighterManager fighterManager;

        public FighterStats CurrentStats { get { return currentStats; } }
        [SerializeField] private FighterStats currentStats = null;

        public void SetStats(FighterStatsHolder statsHolder)
        {
            currentStats = new FighterStats(statsHolder.stats);
        }
    }
}