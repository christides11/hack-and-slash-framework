using System.Collections;
using System.Collections.Generic;
using TDAction.Fighter;
using UnityEngine;

namespace TDAction.Combat
{
    [CreateAssetMenu(fileName = "MovesetDefinition", menuName = "TDA/Combat/Moveset")]
    public class MovesetDefinition : CAF.Combat.MovesetDefinition
    {
        public FighterStatsHolder fighterStats;
        public AnimationReferenceHolder animations;
    }
}