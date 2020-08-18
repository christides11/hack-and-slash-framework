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
            GetEntityManager().PhysicsManager.SetForceDirect(Vector3.zero, Vector3.zero);
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