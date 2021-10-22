using UnityEngine;

namespace TDAction.Combat
{
    [CreateAssetMenu(fileName = "AttackDefinition", menuName = "TDA/Combat/Attack Definition")]
    public class AttackDefinition : HnSF.Combat.AttackDefinition
    {
        [Header("Animation")]
        public string animationName;
    }
}