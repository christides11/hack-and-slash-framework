using CAF.Camera;
using CAF.Simulation;
using TAPI.Combat;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityController : SimObject, ITargetable
    {
        public EntityInputManager InputManager { get { return entityInput; } }
        public EntityStateManager StateManager { get { return entityStateManager; } }
        public EntityCombatManager CombatManager { get { return entityCombatManager; } }
        public EntityPhysicsManager PhysicsManager { get { return entityPhysicsManager; } }

        public virtual bool Targetable { get { return false; } }
        public virtual Vector3 Center { get { return new Vector3(0, 0, 0); } }
        public bool IsGrounded { get; set; } = false;

        [Header("References")]
        [SerializeField] protected EntityInputManager entityInput;
        [SerializeField] protected EntityStateManager entityStateManager;
        [SerializeField] protected EntityCombatManager entityCombatManager;
        [SerializeField] protected EntityPhysicsManager entityPhysicsManager;
        public CapsuleCollider coll;
        public GameObject visual;
        public LookHandler lookHandler;

        /// <summary>
        /// Handles finding and locking on to targets.
        /// </summary>
        protected virtual void HandleLockon()
        {
        }
    }
} 