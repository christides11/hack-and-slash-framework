using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStateKnockdown : FighterState
    {
        public override string GetName()
        {
            return $"Knockdown";
        }

        public override void Initialize()
        {
            GetPhysicsManager().forceMovement = Vector3.zero;
            GetPhysicsManager().forceGravity = Vector3.zero;
        }

        public override void OnUpdate()
        {

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            return false;
        }
    }
}