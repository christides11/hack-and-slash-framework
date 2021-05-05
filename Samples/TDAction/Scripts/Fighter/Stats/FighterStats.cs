using UnityEngine;

namespace TDAction.Fighter
{
    [System.Serializable]
    public class FighterStats
    {
        [SerializeField] public FighterStatFloat health = new FighterStatFloat(100);
        public int enemyStepLength = 4;

        [Header("Ground")]
        public FighterStatFloat groundFriction = new FighterStatFloat(0);

        [Header("Walk")]
        public FighterStatFloat walkBaseAcceleration = new FighterStatFloat(0);
        public FighterStatFloat walkAcceleration = new FighterStatFloat(0);
        public FighterStatFloat walkMaxSpeed = new FighterStatFloat(0);

        [Header("Run")]
        public FighterStatFloat runBaseAcceleration = new FighterStatFloat(0);
        public FighterStatFloat runAcceleration = new FighterStatFloat(0);
        public FighterStatFloat runMaxSpeed = new FighterStatFloat(0);

        [Header("Aerial")]
        public int jumpSquatFrames = 4;
        public FighterStatFloat gravity = new FighterStatFloat(0);
        public FighterStatFloat maxFallSpeed = new FighterStatFloat(0);
        public FighterStatFloat airBaseAcceleration = new FighterStatFloat(0);
        public FighterStatFloat airAcceleration = new FighterStatFloat(0);
        public FighterStatFloat airMaxSpeed = new FighterStatFloat(0);
        public FighterStatFloat aerialFriction = new FighterStatFloat(0);
        public FighterStatFloat shortHopForce = new FighterStatFloat(0);
        public FighterStatFloat fullHopForce = new FighterStatFloat(0);
        public FighterStatFloat airJumpForce = new FighterStatFloat(0);

        [Header("Hitstun")]
        public FighterStatFloat hitstunGravity = new FighterStatFloat(0);
        public FighterStatFloat hitstunMaxFallSpeed = new FighterStatFloat(0);
        public FighterStatFloat hitstunFrictionGround = new FighterStatFloat(0);
        public FighterStatFloat hitstunFrictionAir = new FighterStatFloat(0);

        public FighterStats()
        {

        }

        public FighterStats(FighterStats other)
        {
            health = new FighterStatFloat(other.health.baseValue);
            enemyStepLength = other.enemyStepLength;

            groundFriction = new FighterStatFloat(other.groundFriction.baseValue);

            walkBaseAcceleration = new FighterStatFloat(other.walkBaseAcceleration.baseValue);
            walkAcceleration = new FighterStatFloat(other.walkAcceleration.baseValue);
            walkMaxSpeed = new FighterStatFloat(other.walkMaxSpeed.baseValue);

            runBaseAcceleration = new FighterStatFloat(other.runBaseAcceleration.baseValue);
            runAcceleration = new FighterStatFloat(other.runAcceleration.baseValue);
            runMaxSpeed = new FighterStatFloat(other.runMaxSpeed.baseValue);

            jumpSquatFrames = other.jumpSquatFrames;
            gravity = new FighterStatFloat(other.gravity.baseValue);
            maxFallSpeed = new FighterStatFloat(other.maxFallSpeed.baseValue);
            airBaseAcceleration = new FighterStatFloat(other.airBaseAcceleration.baseValue);
            airAcceleration = new FighterStatFloat(other.airAcceleration.baseValue);
            airMaxSpeed = new FighterStatFloat(other.airMaxSpeed.baseValue);
            aerialFriction = new FighterStatFloat(other.aerialFriction.baseValue);
            shortHopForce = new FighterStatFloat(other.shortHopForce.baseValue);
            fullHopForce = new FighterStatFloat(other.fullHopForce.baseValue);
            airJumpForce = new FighterStatFloat(other.airJumpForce.baseValue);

            hitstunGravity = new FighterStatFloat(other.hitstunGravity.baseValue);
            hitstunMaxFallSpeed = new FighterStatFloat(other.hitstunMaxFallSpeed.baseValue);
            hitstunFrictionGround = new FighterStatFloat(other.hitstunFrictionGround.baseValue);
            hitstunFrictionAir = new FighterStatFloat(other.hitstunFrictionAir.baseValue);
        }
    }
}