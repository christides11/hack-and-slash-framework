using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    [CreateAssetMenu(fileName = "AttackDefinition", menuName = "CAF/Combat/Attack Definition")]
    public class AttackDefinition : ScriptableObject
    {
        #region General
        public string attackName;
        public string description;
        public int stateOverride = -1;
        public int length; //In frames
        public float heightRestriction;
        public float gravityScaleAdded = 0;
        #endregion

        #region Animation
        public AnimationClip animationGround;
        public AnimationClip animationAir;
        public WrapMode wrapMode;
        #endregion

        #region Cancel Windows
        public List<Vector2Int> jumpCancelWindows = new List<Vector2Int>();
        public List<Vector2Int> enemyStepWindows = new List<Vector2Int>();
        public List<Vector2Int> landCancelWindows = new List<Vector2Int>();
        public List<Vector2Int> commandAttackCancelWindows = new List<Vector2Int>();
        #endregion

        public List<BoxGroup> boxGroups = new List<BoxGroup>();

        public List<AttackEventDefinition> events = new List<AttackEventDefinition>();
    }
}