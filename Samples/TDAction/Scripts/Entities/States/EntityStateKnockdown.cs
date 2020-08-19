using UnityEngine;

namespace TDAction.Entities.States
{
    public class EntityStateKnockdown : EntityState
    {
        public override string GetName()
        {
            return $"Knockdown";
        }

        public override void Initialize()
        {
            GetEntityManager().PhysicsManager.forceMovement = Vector3.zero;
            GetEntityManager().PhysicsManager.forceGravity = Vector3.zero;
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