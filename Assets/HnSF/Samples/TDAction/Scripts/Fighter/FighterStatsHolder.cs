using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    [CreateAssetMenu(fileName = "FighterStats", menuName = "TDA/FighterStats")]
    public class FighterStatsHolder : ScriptableObject
    {
        [SerializeField] public FighterStats stats = new FighterStats();
    }
}