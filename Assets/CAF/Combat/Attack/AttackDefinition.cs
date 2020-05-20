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
        public bool possibleGround;
        public float gravityScaleAdded = 0;
        #endregion

        #region Animation
        public AnimationClip animationGround;
        public AnimationClip animationAir;
        public WrapMode wrapMode;
        #endregion

        #region Cancels
        public List<Vector2Int> jumpCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> enemyStepFrames = new List<Vector2Int>();
        public List<Vector2Int> landCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> attackCancelFrames = new List<Vector2Int>();
        #endregion

        public List<BoxGroup> boxGroups = new List<BoxGroup>();

        public List<AttackEventDefinition> events = new List<AttackEventDefinition>();
    }
}